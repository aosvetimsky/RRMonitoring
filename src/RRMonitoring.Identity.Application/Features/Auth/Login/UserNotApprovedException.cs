using System;

namespace RRMonitoring.Identity.Application.Features.Auth.Login;

internal sealed class UserNotApprovedException : Exception
{
	public UserNotApprovedException()
	{
	}

	public UserNotApprovedException(string message, Exception exception = null)
		: base(message, exception)
	{
	}
}
