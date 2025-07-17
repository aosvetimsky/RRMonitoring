using System.Collections.Generic;
using System.Threading.Tasks;
using RRMonitoring.Notification.Application.Providers.Models;

namespace RRMonitoring.Notification.Application.Providers;

public interface INotificationProvider
{
	Task<IList<NotificationResult>> SendNotification(NotificationInfo notificationInfo);

	NotificationResult ReceiveCallback(CallbackInfo callbackInfo);
}
