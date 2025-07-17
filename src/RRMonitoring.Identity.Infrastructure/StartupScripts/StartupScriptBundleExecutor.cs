using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RRMonitoring.Identity.Infrastructure.StartupScripts.PostgreSql;
using RRMonitoring.Identity.Infrastructure.StartupScripts.Settings;

namespace RRMonitoring.Identity.Infrastructure.StartupScripts;

internal sealed class StartupScriptBundleExecutor
{
	private readonly ILogger<StartupScriptBundleExecutor> _logger;

	public StartupScriptBundleExecutor(ILogger<StartupScriptBundleExecutor> logger)
	{
		_logger = logger;
	}

	public async Task ExecuteStartupScripts(
		ITransactionalSqlQueryExecutor sqlQueryExecutor,
		StartupScriptBundleSettings bundleSettings,
		CancellationToken cancellationToken
	)
	{
		var buffer = new Memory<char>(new char[bundleSettings.BufferCharCount]);

		await sqlQueryExecutor.ExecuteInTransaction(async (executeSqlQuery, cancellationToken) =>
		{
			foreach (var sourceSettings in bundleSettings.Sources)
			{
				var normalizedSourceSettings = TryGetNormalizedSourceSettings(bundleSettings, sourceSettings);
				if (normalizedSourceSettings is null)
				{
					continue;
				}

				var files = GetFiles(normalizedSourceSettings);

				if (files.Count == 0)
				{
					_logger.LogWarning("No files found in '{FolderPath}'", normalizedSourceSettings.FolderFullPath);
					continue;
				}

				foreach (var file in files)
				{
					var content = await ReadScriptFile(buffer, file, cancellationToken);

					IEnumerable<StartupScriptSqlQuery> sqlQueries;
					if (file.SourceSettings.UseQuerySplitter)
					{
						sqlQueries = PostgreSqlQuerySplitter.Split(content, (int lineNumber, int column, string message) =>
						{
							return new StartupScriptException($"{message} at {file.FullPath}:{lineNumber}:{column}");
						});
					}
					else
					{
						sqlQueries = new StartupScriptSqlQuery[]
						{
							new() { Text = content, StartLineNumber = 1, StartColumn = 1 },
						};
					}

					var hasSqlQuery = false;

					foreach (var sqlQuery in sqlQueries)
					{
						hasSqlQuery = true;

						_logger.LogInformation(
							"Executing query at {FilePath}:{LineNumber}:{Column}",
							file.FullPath,
							sqlQuery.StartLineNumber,
							sqlQuery.StartColumn
						);
						await executeSqlQuery(sqlQuery.Text, cancellationToken);
					}

					if (!hasSqlQuery)
					{
						_logger.LogWarning("No SQL queries found in '{FilePath}'", file.FullPath);
					}
				}
			}
		}, cancellationToken);
	}

	private sealed record NormalizedSourceSettings
	{
		public string FolderFullPath { get; init; }
		public int MaxRecursionDepth { get; init; }
		public int MaxFileCount { get; init; }
		public Regex RelativePathRegex { get; init; }
		public Regex PositionFileNameRegex { get; init; }
		public int MaxFileCharCount { get; init; }
		public bool UseQuerySplitter { get; init; }
	}

	private static NormalizedSourceSettings TryGetNormalizedSourceSettings(
		StartupScriptBundleSettings bundleSettings,
		StartupScriptSourceSettings sourceSettings
	)
	{
		var enabled = bundleSettings.Enabled && sourceSettings.Enabled;
		if (!enabled)
		{
			return null;
		}

		var folderPath = sourceSettings.FolderPath;
		if (string.IsNullOrWhiteSpace(folderPath))
		{
			throw new StartupScriptException(
				$"'{nameof(StartupScriptSourceSettings.FolderPath)}' setting is not set"
			);
		}

		var folderFullPath = Path.GetFullPath(folderPath);

		if (!Directory.Exists(folderFullPath))
		{
			throw new StartupScriptException(
				$"Invalid '{nameof(StartupScriptSourceSettings.FolderPath)}' setting: '{folderFullPath}' does not exist"
			);
		}

		var maxFileCount = sourceSettings.MaxFileCount ?? bundleSettings.DefaultMaxFileCountInFolder;

		var maxRecursionDepth = sourceSettings.MaxRecursionDepth ?? bundleSettings.DefaultMaxRecursionDepth;

		var relativePathRegexPattern = sourceSettings.RelativePathRegexPattern ?? bundleSettings.DefaultRelativePathRegexPattern;
		var relativePathRegex = MakeRegex(relativePathRegexPattern);

		var positionFileNameRegexPattern = sourceSettings.PositionFileNameRegexPattern ?? bundleSettings.DefaultPositionFileNameRegexPattern;
		var positionFileNameRegex = MakeRegex(positionFileNameRegexPattern);

		var maxFileCharCount = sourceSettings.MaxFileCharCount ?? bundleSettings.DefaultMaxFileCharCount;

		var useQuerySplitter = sourceSettings.UseQuerySplitter ?? bundleSettings.DefaultUseQuerySplitter;

		return new NormalizedSourceSettings
		{
			FolderFullPath = folderFullPath,
			MaxRecursionDepth = maxRecursionDepth,
			MaxFileCount = maxFileCount,
			RelativePathRegex = relativePathRegex,
			PositionFileNameRegex = positionFileNameRegex,
			MaxFileCharCount = maxFileCharCount,
			UseQuerySplitter = useQuerySplitter,
		};
	}

	private static Regex MakeRegex(string pattern)
	{
		const RegexOptions regexOptions =
			RegexOptions.ExplicitCapture |
			RegexOptions.Multiline |
			RegexOptions.Compiled |
			RegexOptions.Singleline |
			RegexOptions.IgnorePatternWhitespace |
			RegexOptions.CultureInvariant |
			RegexOptions.None;

		return new Regex($@"\A({pattern})\z", regexOptions);
	}

	private sealed record StartupScriptFile
	{
		public NormalizedSourceSettings SourceSettings { get; init; }
		public string FullPath { get; init; }
		public string RelativePath { get; init; }
		public int Position { get; init; }
	};

	private IReadOnlyList<StartupScriptFile> GetFiles(NormalizedSourceSettings folderSettings)
	{
		var folder = new DirectoryInfo(folderSettings.FolderFullPath);

		var files = new List<StartupScriptFile>();

		var enumerationOptions = new EnumerationOptions
		{
			AttributesToSkip = 0,
			IgnoreInaccessible = false,
			MatchCasing = MatchCasing.CaseSensitive,
			MatchType = MatchType.Simple,
			MaxRecursionDepth = folderSettings.MaxRecursionDepth,
			RecurseSubdirectories = true,
			ReturnSpecialDirectories = false,
		};
		foreach (var fileInfo in folder.EnumerateFiles("*", enumerationOptions))
		{
			var fullPath = fileInfo.FullName;
			var fileName = fileInfo.Name;

			var relativePath = Path.GetRelativePath(folderSettings.FolderFullPath, fullPath).Replace('\\', '/');

			if (!folderSettings.RelativePathRegex.IsMatch(relativePath))
			{
				_logger.LogDebug("Skipped {RelativePath} in {FolderFullPath}", relativePath, folderSettings.FolderFullPath);
				continue;
			}

			var position = TryGetPosition(fileName);
			if (position is null)
			{
				throw new StartupScriptException(
					$"No position information or invalid file name '{relativePath}' in '{folderSettings.FolderFullPath}'"
				);
			}

			if (files.Count >= folderSettings.MaxFileCount)
			{
				throw new StartupScriptException(
					$"Too many files in '{folderSettings.FolderFullPath}', limit is {folderSettings.MaxFileCount}"
				);
			}

			var file = new StartupScriptFile
			{
				SourceSettings = folderSettings,
				FullPath = fullPath,
				RelativePath = relativePath,
				Position = position.Value,
			};
			files.Add(file);
		}

		int? TryGetPosition(string fileName)
		{
			var match = folderSettings.PositionFileNameRegex.Match(fileName);
			var positionString = match.Groups["position"].Value;
			if (!int.TryParse(positionString, NumberStyles.None, CultureInfo.InvariantCulture, out var position))
			{
				return null;
			}

			return position;
		}

		files = files.OrderBy(x => x.Position).ToList();

		StartupScriptFile previousFile = null;
		foreach (var file in files)
		{
			if (previousFile is not null && previousFile.Position == file.Position)
			{
				throw new StartupScriptException(
					$"Folder '{folderSettings.FolderFullPath}' has files with same position {file.Position}: '{previousFile.RelativePath}' and '{file.RelativePath}'"
				);
			}

			previousFile = file;
		}

		return files;
	}

	private static readonly Encoding _scriptFileEncoding = new UTF8Encoding(
		encoderShouldEmitUTF8Identifier: true,
		throwOnInvalidBytes: true
	);

	private static async Task<string> ReadScriptFile(Memory<char> buffer, StartupScriptFile file, CancellationToken cancellationToken)
	{
		using var streamReader = new StreamReader(file.FullPath, _scriptFileEncoding, detectEncodingFromByteOrderMarks: true);

		var textContentBuilder = new StringBuilder();

		while (true)
		{
			var charsRead = await streamReader.ReadAsync(buffer, cancellationToken);
			if (charsRead == 0)
			{
				break;
			}

			var totalCharCount = textContentBuilder.Length + charsRead;
			if (totalCharCount > file.SourceSettings.MaxFileCharCount)
			{
				throw new StartupScriptException(
					$"Number of characters in '{file.FullPath}' is greater than {file.SourceSettings.MaxFileCharCount}"
				);
			}

			textContentBuilder.Append(buffer.Slice(0, charsRead));
		}

		textContentBuilder.Replace("\r\n", "\n");
		textContentBuilder.Replace('\r', '\n');

		return textContentBuilder.ToString();
	}
}
