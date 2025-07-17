using System;
using System.Threading.Tasks;
using Nomium.Core.Data.Repositories;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Domain.Contracts;

public interface ITemplateRepository : IRepository<Template, Guid>
{
	Task<Template> Get(Channels channel, Guid notificationId);
}
