using RRMonitoring.Identity.Application.Enums;

namespace RRMonitoring.Identity.Application.Features.Auth.LoginOrRegisterExternal;

public class ExternalLoginResultDto
{
	public bool IsSuccess { get; set; }

	public string ErrorMessage { get; set; }

	public ExternalAuthErrorCodes? ErrorCode { get; set; }

	public static ExternalLoginResultDto Failed(string message, ExternalAuthErrorCodes? errorCode = null)
	{
		return new ExternalLoginResultDto
		{
			ErrorMessage = message,
			ErrorCode = errorCode
		};
	}

	public static ExternalLoginResultDto Success()
	{
		return new ExternalLoginResultDto
		{
			IsSuccess = true
		};
	}
}
