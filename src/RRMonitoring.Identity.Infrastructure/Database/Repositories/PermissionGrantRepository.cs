using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nomium.Core.Data.EntityFrameworkCore.Extensions;
using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Infrastructure.Database.Repositories;

internal sealed class PermissionGrantRepository : RepositoryBase<PermissionGrant, Guid>, IPermissionGrantRepository
{
	public PermissionGrantRepository(IdentityContext identityContext)
		: base(identityContext)
	{
	}

	public Task<List<PermissionGrantPermission>> GetUserActiveGrantedPermissionsByDate(Guid userId, DateTime dateTime)
	{
		return EntitiesDbSet
			.Where(x => x.StartDate <= dateTime &&
			            x.EndDate >= dateTime &&
			            x.DestinationUserId == userId)
			.SelectMany(x => x.GrantedPermissions)
			.ToListAsync();
	}

	public Task<List<PermissionGrant>> Search(SearchPermissionGrantsCriteria criteria, string[] includePaths = null)
	{
		return BuildSearchQuery(criteria, includePaths)
			.ToListAsync();
	}

	public Task<PagedList<PermissionGrant>> SearchWithPaging(
		SearchPermissionGrantsPagedCriteria criteria, string[] includePaths = null)
	{
		return BuildSearchQuery(criteria, includePaths)
			.ToSearchResult(criteria.SortExpressions, criteria.Skip, criteria.Take);
	}

	private IQueryable<PermissionGrant> BuildSearchQuery(
		ISearchPermissionGrantsCriteria criteria, string[] includePaths = null)
	{
		var query = EntitiesDbSet
			.AddIncludes<PermissionGrant, Guid>(includePaths)
			.AsNoTracking();

		if (criteria.SourceUserIds != null)
		{
			query = query.Where(x => criteria.SourceUserIds.Contains(x.SourceUserId));
		}

		if (criteria.DestinationUserIds != null)
		{
			query = query.Where(x => criteria.DestinationUserIds.Contains(x.DestinationUserId));
		}

		if (criteria.PermissionIds != null)
		{
			query = query.Where(x =>
				x.GrantedPermissions.Any(grantedPermission =>
					criteria.PermissionIds.Contains(grantedPermission.PermissionId)));
		}

		if (criteria.GrantDates.HasValue)
		{
			query = query.Where(x =>
				x.StartDate >= criteria.GrantDates.Value.StartDateTime ||
				x.EndDate >= criteria.GrantDates.Value.StartDateTime);

			query = query.Where(x =>
				x.StartDate <= criteria.GrantDates.Value.EndDateTime ||
				x.EndDate <= criteria.GrantDates.Value.EndDateTime);
		}

		return query;
	}
}
