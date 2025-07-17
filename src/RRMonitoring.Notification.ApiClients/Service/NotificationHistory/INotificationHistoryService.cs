using System;
using System.Threading.Tasks;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Notification.ApiClients.Service.NotificationHistory;

public interface INotificationHistoryService
{
	Task<DateTime?> GetNotificationLastSentDate<TNotificationBase>(Guid recipientId)
		where TNotificationBase : NotificationBase;
}
