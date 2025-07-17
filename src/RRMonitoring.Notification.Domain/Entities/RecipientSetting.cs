namespace RRMonitoring.Notification.Domain.Entities;

public class RecipientSetting
{
	public string RecipientId { get; set; }

	public byte ChannelId { get; set; }
	public Channel Channel { get; set; }

	public string NotificationIdentifier { get; set; }
	public Notification Notification { get; set; }

	public bool IsEnabled { get; set; }
}

