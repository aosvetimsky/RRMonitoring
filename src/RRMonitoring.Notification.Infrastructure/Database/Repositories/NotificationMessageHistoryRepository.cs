using System;
using System.Linq;
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

public class NotificationMessageHistoryRepository(NotificationContext context)
	: RepositoryBase<NotificationMessageHistory, Guid>(context),
		INotificationMessageHistoryRepository
{
	public async Task<PagedList<NotificationMessageHistory>> Search(SearchNotificationMessageHistoryCriteria criteria)
	{
		var query = EntitiesDbSet
			.Include(x => x.NotificationMessage)
			.ThenInclude(x => x.Notification)
			.AsNoTracking();

		if (!string.IsNullOrEmpty(criteria.Keyword))
		{
			query = query.Where(
				x => EF.Functions.ILike(x.NotificationMessage.RecipientAddress, $"%{criteria.Keyword}%"));
		}

		if (criteria.Channels != null)
		{
			query = query.Where(x => criteria.Channels.Contains((Channels)x.NotificationMessage.ChannelId));
		}

		if (criteria.DatePeriod != null)
		{
			var datePeriod = criteria.DatePeriod.Value;
			query = query.Where(x =>
				x.CreatedDate >= datePeriod.StartDateTime && x.CreatedDate <= datePeriod.EndDateTime);
		}

		if (criteria.Statuses != null)
		{
			query = query.Where(x => criteria.Statuses.Contains((NotificationStatuses)x.StatusId));
		}

		return await query
			.ToSearchResult(criteria.SortExpressions, criteria.Skip, criteria.Take);
	}

	public async Task<DateTime?> GetNotificationLastSentDate(string recipientId, string notificationIdentifier)
	{
		var result = await EntitiesDbSet
			.AsNoTracking()
			.Where(x =>
				x.NotificationMessage.RecipientId == recipientId
				&& x.NotificationMessage.Notification.Identifier == notificationIdentifier)
			.OrderByDescending(x => x.CreatedDate)
			.Select(x => x.CreatedDate)
			.FirstOrDefaultAsync();

		if (result == default)
		{
			return null;
		}

		return result;
	}
}
