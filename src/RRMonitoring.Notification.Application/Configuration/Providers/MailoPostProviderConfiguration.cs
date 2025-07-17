namespace RRMonitoring.Notification.Application.Configuration.Providers;

public sealed class MailoPostProviderConfiguration
{
	public string Url { get; set; }

	public string SenderEmail { get; set; }

	public string Token { get; set; }

	public string Payment { get; set; }
}
