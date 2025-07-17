namespace RRMonitoring.Identity.Application.Configuration;

public class TimeoutConfig
{
	public int SendChangeEmailCodeTimeout { get; set; }

	public int SendResetPasswordCodeTimeout { get; set; }

	public int SendRegistrationCodeTimeout { get; set; }

	public int SendTwoFactorCodeTimeout { get; set; }

	public int SendChangePhoneCodeTimeout { get; set; }

	public int FunctionsBlockTimeInHours { get; set; }
}
