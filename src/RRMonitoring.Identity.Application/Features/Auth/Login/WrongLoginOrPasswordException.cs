using System;

namespace RRMonitoring.Identity.Application.Features.Auth.Login;

internal class WrongLoginOrPasswordException : Exception
{
	public WrongLoginOrPasswordException()
	{
	}

	public WrongLoginOrPasswordException(string message, Exception exception = null)
		: base(message, exception)
	{
	}
}
