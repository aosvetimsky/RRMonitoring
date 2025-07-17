using System;
using System.Threading.Tasks;
using System.Threading;
using Nomium.Core.Data.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Equipment.Domain.Models.Manufacturer;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Domain.Contracts.Repositories;

public interface IManufacturerRepository : IRepository<Manufacturer, Guid>
{
	Task<Manufacturer> GetByName(string name, CancellationToken cancellationToken);

	Task<PagedList<Manufacturer>> Search(SearchManufacturersCriteria criteria, CancellationToken cancellationToken);
}