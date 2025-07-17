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
using RRMonitoring.Mining.Domain.Models.Worker;

namespace RRMonitoring.Mining.Infrastructure.Database.Repositories;

internal class WorkerRepository(MiningContext context) : RepositoryBase<Worker, Guid>(context), IWorkerRepository
{
	public async Task<PagedList<Worker>> Search(SearchWorkersCriteria criteria, CancellationToken cancellationToken)
	{
		var query = EntitiesDbSet
			.Include(x => x.Client)
			.Include(x => x.Pool)
			.Include(x => x.Coin)
			.AsNoTracking();

		if (!string.IsNullOrEmpty(criteria.Keyword))
		{
			query = query.Where(x => EF.Functions.ILike(x.Name, $"%{criteria.Keyword}%") || EF.Functions.ILike(x.Client.Name, $"%{criteria.Keyword}%"));
		}

		if (criteria.PoolIds is not null)
		{
			query = query.Where(x => criteria.PoolIds.Contains(x.PoolId));
		}

		if (criteria.CoinIds is not null)
		{
			query = query.Where(x => criteria.CoinIds.Contains(x.CoinId));
		}

		if (criteria.StatusIds is not null)
		{
			query = query.Where(x => criteria.StatusIds.Contains(x.StatusId));
		}

		return await query.ToSearchResult(criteria.SortExpressions, criteria.Skip, criteria.Take, cancellationToken);
	}
}
