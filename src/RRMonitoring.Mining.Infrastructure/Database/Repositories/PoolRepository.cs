using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nomium.Core.Data.EntityFrameworkCore.Extensions;
using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Mining.Domain.Contracts.Repositories;
using RRMonitoring.Mining.Domain.Entities;
using RRMonitoring.Mining.Domain.Models.Pool;

namespace RRMonitoring.Mining.Infrastructure.Database.Repositories;

internal class PoolRepository(MiningContext context) : RepositoryBase<Pool, Guid>(context), IPoolRepository
{
	public async Task<Pool> GetByName(string name, CancellationToken cancellationToken)
	{
		return await EntitiesDbSet
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
	}

	public async Task<PagedList<Pool>> Search(SearchPoolsCriteria criteria, CancellationToken cancellationToken)
	{
		var query = EntitiesDbSet
			.Include(p => p.CoinAddresses)
				.ThenInclude(p => p.Coin)
			.AsNoTracking();

		if (!string.IsNullOrEmpty(criteria.Keyword))
		{
			query = query.Where(p => EF.Functions.ILike(p.Name, $"%{criteria.Keyword}%"));
		}

		return await query.ToSearchResult(criteria.SortExpressions, criteria.Skip, criteria.Take, cancellationToken);
	}
}
