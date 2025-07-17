namespace RRMonitoring.Identity.Infrastructure.StartupScripts.Settings;

internal sealed record StartupScriptSourceSettings
{
	public bool Enabled { get; init; }

	public string FolderPath { get; init; }

	public int? MaxRecursionDepth { get; init; }

	public int? MaxFileCount { get; init; }

	public string RelativePathRegexPattern { get; init; }

	public string PositionFileNameRegexPattern { get; init; }

	public int? MaxFileCharCount { get; init; }

	public bool? UseQuerySplitter { get; init; }
}
