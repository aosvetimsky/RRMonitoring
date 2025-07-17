using RRMonitoring.Notification.ApiClients.Enums;

namespace RRMonitoring.Notification.ApiClients.Models;

public abstract class PushNotification : NotificationBase
{
	protected PushNotification()
		: base(Channels.Push)
	{
	}
}
