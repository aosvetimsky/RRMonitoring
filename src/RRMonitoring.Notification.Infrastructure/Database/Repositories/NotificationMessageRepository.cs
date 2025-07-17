using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nomium.Core.Data.EntityFrameworkCore.Extensions;
using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;
using RRMonitoring.Notification.Domain.Models;

namespace RRMonitoring.Notification.Infrastructure.Database.Repositories;

public class NotificationMessageRepository(NotificationContext context)
	: RepositoryBase<NotificationMessage, Guid>(context),
		INotificationMessageRepository
{
	public async Task<NotificationMessage> GetByExternalId(
		Channels channel, string externalId, CancellationToken cancellationToken)
	{
		return await EntitiesDbSet
			.Where(x => x.ExternalMessageId == externalId)
			.Where(x => x.ChannelId == (byte)channel)
			.AsNoTracking()
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<PagedList<NotificationMessage>> Search(
		SearchNotificationMessageCriteria criteria, CancellationToken cancellationToken)
	{
		var query = EntitiesDbSet
			.AsNoTracking();

		if (criteria.RecipientIds != null)
		{
			query = query.Where(x => criteria.RecipientIds.Contains(x.RecipientId));
		}

		if (criteria.Channels != null)
		{
			var channelIds = criteria.Channels.Cast<byte>().ToList();
			query = query.Where(x => channelIds.Contains(x.ChannelId));
		}

		if (criteria.IncludeNotification)
		{
			query = query.Include(x => x.Notification);
		}

		return await query.ToSearchResult(
			criteria.SortExpressions,
			criteria.Skip,
			criteria.Take,
			cancellationToken);
	}
}
