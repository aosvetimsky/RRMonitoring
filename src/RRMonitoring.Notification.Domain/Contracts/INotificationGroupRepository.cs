using Nomium.Core.Data.Repositories;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Domain.Contracts;

public interface INotificationGroupRepository : IRepository<NotificationGroup, int>;
