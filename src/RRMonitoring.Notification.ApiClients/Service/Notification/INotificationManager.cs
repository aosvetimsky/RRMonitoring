using System.Collections.Generic;
using System.Threading.Tasks;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Notification.ApiClients.Service.Notification;

public interface INotificationManager
{
	Task SendEmail(EmailNotification notification);

	Task SendSms(SmsNotification notification);

	Task SendPush(PushNotification notification);

	Task SendMultiple(IList<NotificationBase> notifications);
}
