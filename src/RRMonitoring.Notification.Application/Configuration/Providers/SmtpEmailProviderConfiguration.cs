namespace RRMonitoring.Notification.Application.Configuration.Providers;

public class SmtpEmailProviderConfiguration
{
	public string SenderEmail { get; set; }

	public string Host { get; set; }

	public int Port { get; set; }

	public string Password { get; set; }

	public bool UseSsl { get; set; }
}
