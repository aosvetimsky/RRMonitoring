namespace RRMonitoring.Notification.ApiClients.Configuration;

public class NotificationHttpConfiguration
{
	public string Url { get; set; }

	public string ApiKey { get; set; }
}

public class NotificationRabbitConfiguration
{
	public string Host { get; set; }

	public ushort Port { get; set; }

	public string VirtualHost { get; set; }

	public string User { get; set; }

	public string Password { get; set; }
}
