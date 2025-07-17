using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Data.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;
using RRMonitoring.Notification.Domain.Models;

namespace RRMonitoring.Notification.Domain.Contracts;

public interface INotificationMessageRepository : IRepository<NotificationMessage, Guid>
{
	Task<NotificationMessage> GetByExternalId(Channels channel, string externalId, CancellationToken cancellationToken);

	Task<PagedList<NotificationMessage>> Search(
		SearchNotificationMessageCriteria criteria, CancellationToken cancellationToken);
}
