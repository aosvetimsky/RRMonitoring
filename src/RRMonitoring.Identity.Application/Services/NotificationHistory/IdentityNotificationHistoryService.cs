using System;
using System.Threading.Tasks;
using Nomium.Core.Application.Services.DateTimeProvider;
using RRMonitoring.Notification.ApiClients.Models;
using RRMonitoring.Notification.ApiClients.Service.NotificationHistory;

namespace RRMonitoring.Identity.Application.Services.NotificationHistory;

public class IdentityNotificationHistoryService : IIdentityNotificationHistoryService
{
	private readonly INotificationHistoryService _notificationHistoryService;
	private readonly IDateTimeProvider _dateTimeProvider;

	public IdentityNotificationHistoryService(
		INotificationHistoryService notificationHistoryService,
		IDateTimeProvider dateTimeProvider)
	{
		_notificationHistoryService = notificationHistoryService;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<int> GetTimeout<T>(Guid recipientId, int maxTimeout) where T : NotificationBase
	{
		var notificationLastSentDate = await _notificationHistoryService.GetNotificationLastSentDate<T>(recipientId);
		if (notificationLastSentDate is null || notificationLastSentDate == DateTime.MinValue)
		{
			return 0;
		}

		var secondsPassed = _dateTimeProvider.GetUtcNow().Subtract((DateTime)notificationLastSentDate).TotalSeconds;

		return (int)(maxTimeout > secondsPassed ? maxTimeout - secondsPassed : 0);
	}
}
