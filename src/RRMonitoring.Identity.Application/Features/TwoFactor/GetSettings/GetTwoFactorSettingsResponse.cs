namespace RRMonitoring.Identity.Application.Features.TwoFactor.GetSettings;

public class GetTwoFactorSettingsResponse
{
	public bool IsAuthenticatorEnabled { get; init; }

	public bool IsPhoneNumberSetuped { get; init; }
}
