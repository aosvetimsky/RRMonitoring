namespace RRMonitoring.Identity.Application.Configuration;

public class AuthenticationConfig
{
	public bool IsTwoFactorAuthenticationEnabled { get; set; }

	public bool IsUserAgreementAcceptanceEnabled { get; set; }

	public bool IsUserLockoutEnabled { get; set; }

	public bool RevokeRefreshTokensOnResetPassword { get; set; }

	public string AgreementUrl { get; set; }
}
