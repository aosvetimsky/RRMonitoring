using System.Collections.Generic;

namespace RRMonitoring.Identity.Infrastructure.StartupScripts.Settings;

internal sealed record StartupScriptBundleSettings
{
	public bool Enabled { get; init; }

	public bool NoCommit { get; init; }

	public int DefaultMaxRecursionDepth { get; init; } = 10;

	public int DefaultMaxFileCountInFolder { get; init; } = 1000;

	// language=regex
	public string DefaultRelativePathRegexPattern { get; init; } = @"([^/]+/)* [^/]+\.(?i:sql)";

	// language=regex
	public string DefaultPositionFileNameRegexPattern { get; init; } = @"(?<position>\d+)-.*\.sql";

	public int BufferCharCount { get; init; } = 10000;

	public int DefaultMaxFileCharCount { get; init; } = 1000000;

	public bool DefaultUseQuerySplitter { get; init; } = true;

	public IReadOnlyList<StartupScriptSourceSettings> Sources { get; init; }
}
