namespace RRMonitoring.Notification.Application.Configuration.Providers;

public sealed class SmsAeroProviderConfiguration
{
	public string Url { get; set; }

	public string Login { get; set; }

	public string ApiKey { get; set; }

	public string SenderName { get; set; }

	public string CallbackUrl { get; set; }
}
