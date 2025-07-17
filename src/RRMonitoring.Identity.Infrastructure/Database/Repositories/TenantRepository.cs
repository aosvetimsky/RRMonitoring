using System;
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

internal class TenantRepository : RepositoryBase<Tenant, Guid>, ITenantRepository
{
	public TenantRepository(IdentityContext context) : base(context)
	{
	}

	public Task<Tenant> GetByCode(string code)
	{
		return EntitiesDbSet
			.AsNoTracking()
			.SingleOrDefaultAsync(x => x.Code == code);
	}

	public Task<Tenant> GetFirstByUserId(Guid userId)
	{
		return Context.Set<TenantUser>()
			.AsNoTracking()
			.Where(userRole => userRole.UserId == userId)
			.Select(userRole => userRole.Tenant)
			.FirstOrDefaultAsync();
	}

	public async Task<PagedList<Tenant>> SearchTenants(SearchTenantsCriteria searchTenantsCriteria)
	{
		var query = EntitiesDbSet.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(searchTenantsCriteria.Keyword))
		{
			query = query.Where(x => EF.Functions.ILike(x.Name, $"{searchTenantsCriteria.Keyword}%"));
		}

		return await query
			.ToSearchResult(
				searchTenantsCriteria.SortExpressions,
				searchTenantsCriteria.Skip,
				searchTenantsCriteria.Take);
	}
}
