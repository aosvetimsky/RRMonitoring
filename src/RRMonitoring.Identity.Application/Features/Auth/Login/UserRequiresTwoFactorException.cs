using System;

namespace RRMonitoring.Identity.Application.Features.Auth.Login;

public class UserRequiresTwoFactorException : Exception
{
	public UserRequiresTwoFactorException()
	{

	}

	public UserRequiresTwoFactorException(string message, Exception exception = null)
		: base(message, exception)
	{
	}
}
