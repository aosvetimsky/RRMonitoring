using System;
using System.Threading.Tasks;
using System.Threading;
using Nomium.Core.Data.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Mining.Domain.Entities;
using RRMonitoring.Mining.Domain.Models.Worker;

namespace RRMonitoring.Mining.Domain.Contracts.Repositories;

public interface IWorkerRepository : IRepository<Worker, Guid>
{
	Task<PagedList<Worker>> Search(SearchWorkersCriteria criteria, CancellationToken cancellationToken);
}
