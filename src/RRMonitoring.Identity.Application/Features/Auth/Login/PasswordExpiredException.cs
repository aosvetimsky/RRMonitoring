using System;

namespace RRMonitoring.Identity.Application.Features.Auth.Login;

public class PasswordExpiredException : Exception
{
	public PasswordExpiredException()
	{
	}

	public PasswordExpiredException(string message, Exception exception = null)
		: base(message, exception)
	{
	}
}
