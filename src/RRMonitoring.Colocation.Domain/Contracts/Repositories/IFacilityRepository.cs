using System;
using System.Threading.Tasks;
using System.Threading;
using Nomium.Core.Data.Repositories;
using RRMonitoring.Colocation.Domain.Entities;
using Nomium.Core.Models;
using RRMonitoring.Colocation.Domain.Models.Facility;

namespace RRMonitoring.Colocation.Domain.Contracts.Repositories;

public interface IFacilityRepository : IRepository<Facility, Guid>
{
	Task<Facility> GetByName(string name, CancellationToken cancellationToken);

	Task<PagedList<Facility>> Search(SearchFacilitiesCriteria criteria, CancellationToken cancellationToken);
}
