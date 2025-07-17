using System;

namespace RRMonitoring.Identity.Application.Features.Auth.Login;

internal sealed class UserLockedExceptions : Exception
{
	public UserLockedExceptions()
	{
	}

	public UserLockedExceptions(string message, Exception exception = null)
		: base(message, exception)
	{
	}
}
