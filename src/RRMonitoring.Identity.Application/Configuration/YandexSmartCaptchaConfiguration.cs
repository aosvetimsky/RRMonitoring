namespace RRMonitoring.Identity.Application.Configuration;

public class YandexSmartCaptchaConfiguration
{
	public bool Enabled { get; set; }

	public bool EnabledOnLogin { get; set; }

	public bool EnabledOnForgotPassword { get; set; }

	public string ClientSecret { get; set; }

	public string ServerSecret { get; set; }
}
