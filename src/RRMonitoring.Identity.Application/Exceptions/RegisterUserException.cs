using Nomium.Core.Exceptions;

namespace RRMonitoring.Identity.Application.Exceptions;

public class RegisterUserException : ValidationException
{
	public RegisterUserException(string message) : base(message)
	{
	}
}
