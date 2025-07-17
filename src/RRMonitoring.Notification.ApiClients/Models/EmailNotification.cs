using RRMonitoring.Notification.ApiClients.Enums;

namespace RRMonitoring.Notification.ApiClients.Models;

public abstract class EmailNotification : NotificationBase
{
	protected EmailNotification() : base(Channels.Email)
	{
	}
}
