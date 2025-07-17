using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Data.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.Domain.Models.Firmware;

namespace RRMonitoring.Equipment.Domain.Contracts.Repositories;

public interface IFirmwareRepository : IRepository<Firmware, Guid>
{
	Task<Firmware> GetByName(string name, CancellationToken cancellationToken);

	Task<PagedList<Firmware>> Search(SearchFirmwareCriteria criteria, CancellationToken cancellationToken);
}
