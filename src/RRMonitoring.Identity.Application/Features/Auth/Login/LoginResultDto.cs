namespace RRMonitoring.Identity.Application.Features.Auth.Login;

public class LoginResultDto
{
	public bool IsSuccess { get; set; }

	public bool IsTwoFactorRequired { get; set; }

	public bool ShouldAcceptAgreement { get; set; }

	public bool ShouldChangePassword { get; set; }

	public string ErrorMessage { get; set; }

	public static LoginResultDto Failed(string message)
	{
		return new LoginResultDto
		{
			ErrorMessage = message
		};
	}

	public static LoginResultDto Success(
		bool isTwoFactorRequired = false,
		bool isAgreementAcceptanceRequired = false,
		bool isChangingPasswordRequired = false)
	{
		return new LoginResultDto
		{
			IsSuccess = true,
			IsTwoFactorRequired = isTwoFactorRequired,
			ShouldAcceptAgreement = isAgreementAcceptanceRequired,
			ShouldChangePassword = isChangingPasswordRequired
		};
	}
}
