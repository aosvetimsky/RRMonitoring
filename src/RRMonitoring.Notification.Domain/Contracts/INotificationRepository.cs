using System;
using System.Threading.Tasks;
using Nomium.Core.Data.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Notification.Domain.Models;

namespace RRMonitoring.Notification.Domain.Contracts;

public interface INotificationRepository : IRepository<Entities.Notification, Guid>
{
	Task<Entities.Notification> GetByIdentifier(string identifier);

	Task<PagedList<Entities.Notification>> Search(SearchNotificationsCriteria criteria);
}
