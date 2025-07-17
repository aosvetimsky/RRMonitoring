using System;
using System.Threading.Tasks;
using RRMonitoring.Notification.ApiClients.ApiClients.Notification.Http;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Notification.ApiClients.Service.NotificationHistory;

internal class NotificationHistoryService : INotificationHistoryService
{
	private readonly INotificationApiClient _notificationApiClient;

	public NotificationHistoryService(INotificationApiClient notificationApiClient)
	{
		_notificationApiClient = notificationApiClient;
	}

	public async Task<DateTime?> GetNotificationLastSentDate<TNotificationBase>(Guid recipientId)
		where TNotificationBase : NotificationBase
	{
		return await _notificationApiClient.GetNotificationLastSentDate<TNotificationBase>(recipientId);
	}
}
