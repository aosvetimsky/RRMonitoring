using System;
using System.Threading.Tasks;
using Nomium.Core.Data.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Models;

namespace RRMonitoring.Notification.Domain.Contracts;

public interface INotificationMessageHistoryRepository
	: IRepository<NotificationMessageHistory, Guid>
{
	Task<PagedList<NotificationMessageHistory>> Search(SearchNotificationMessageHistoryCriteria criteria);

	Task<DateTime?> GetNotificationLastSentDate(string recipientId, string notificationIdentifier);
}
