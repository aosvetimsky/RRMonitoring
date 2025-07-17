using System.Collections.Generic;
using System.Threading.Tasks;
using RRMonitoring.Notification.ApiClients.ApiClients.Notification.Models;

namespace RRMonitoring.Notification.ApiClients.ApiClients.Notification;

internal interface INotificationProvider
{
	Task SendMultiple(IList<SendNotificationRequest> requests);
}
