using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Data.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Domain.Contracts.Repositories;

public interface ICountryRepository : IRepository<Country, int>
{
	Task<List<Country>> GetActive(CancellationToken cancellationToken);
}
