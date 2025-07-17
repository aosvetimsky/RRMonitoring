using System;

namespace RRMonitoring.Identity.Infrastructure.StartupScripts;

internal sealed class StartupScriptException : InvalidOperationException
{
	public StartupScriptException(string message, Exception innerException = null)
		: base(message, innerException)
	{
	}
}
