namespace RRMonitoring.Identity.Infrastructure.StartupScripts;

internal sealed record StartupScriptSqlQuery
{
	public string Text { get; init; }

	public int StartLineNumber { get; init; }

	public int StartColumn { get; init; }
}
