using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nomium.Core.Data.EntityFrameworkCore.Extensions;
using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Models;

namespace RRMonitoring.Notification.Infrastructure.Database.Repositories;

public class NotificationRepository(NotificationContext context)
	: RepositoryBase<Domain.Entities.Notification, Guid>(context),
		INotificationRepository
{
	public async Task<Domain.Entities.Notification> GetByIdentifier(string identifier)
	{
		return await EntitiesDbSet
			.SingleOrDefaultAsync(x => x.Identifier == identifier);
	}

	public async Task<PagedList<Domain.Entities.Notification>> Search(SearchNotificationsCriteria criteria)
	{
		var query = EntitiesDbSet
			.AsNoTracking();

		if (!string.IsNullOrEmpty(criteria.Keyword))
		{
			query = query.Where(x => EF.Functions.ILike(x.Identifier, $"%{criteria.Keyword}%"));
		}

		return await query
			.ToSearchResult(criteria.SortExpressions, criteria.Skip, criteria.Take);
	}
}
