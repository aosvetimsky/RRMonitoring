using System;
using System.Threading.Tasks;
using LinqToDB;
using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Infrastructure.Database.Repositories;

public class TemplateRepository(NotificationContext context)
	: RepositoryBase<Template, Guid>(context),
		ITemplateRepository
{
	public async Task<Template> Get(Channels channel, Guid notificationId)
	{
		return await EntitiesDbSet
			.SingleOrDefaultAsync(x => x.ChannelId == (byte)channel && x.NotificationId == notificationId);
	}
}
