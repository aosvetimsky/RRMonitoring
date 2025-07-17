using RRMonitoring.Notification.ApiClients.Enums;

namespace RRMonitoring.Notification.ApiClients.Models;

public abstract class SmsNotification : NotificationBase
{
	protected SmsNotification()
		: base(Channels.Sms)
	{
	}
}
