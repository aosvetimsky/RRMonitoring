using System.Collections.Generic;

namespace RRMonitoring.Notification.BusEvents.Notifications;

public sealed class SendNotificationEvent
{
	public List<NotificationEventItem> Items { get; set; }
}

public sealed class NotificationEventItem
{
	public string Identifier { get; set; }

	public Channels Channel { get; set; }

	public string Recipient { get; set; }

	public string RecipientId { get; set; }

	public string UserData { get; set; }
}

public enum Channels : byte
{
	Email = 1,
	Push = 2,
	Sms = 3
}
