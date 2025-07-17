using System;
using System.Threading.Tasks;
using System.Threading;
using Nomium.Core.Data.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Mining.Domain.Entities;
using RRMonitoring.Mining.Domain.Models.Pool;

namespace RRMonitoring.Mining.Domain.Contracts.Repositories;

public interface IPoolRepository : IRepository<Pool, Guid>
{
	Task<Pool> GetByName(string name, CancellationToken cancellationToken);

	Task<PagedList<Pool>> Search(SearchPoolsCriteria criteria, CancellationToken cancellationToken);
}
