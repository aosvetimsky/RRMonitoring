using System;
using System.Threading.Tasks;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Notification.ApiClients.ApiClients.Notification.Http;

internal interface INotificationApiClient : INotificationProvider
{
	Task<DateTime?> GetNotificationLastSentDate<TNotificationBase>(Guid recipientId)
		where TNotificationBase : NotificationBase;
}
