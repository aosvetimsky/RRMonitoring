using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Infrastructure.Database.Repositories;

public class NotificationGroupRepository(NotificationContext context)
	: RepositoryBase<NotificationGroup, int>(context), INotificationGroupRepository;
