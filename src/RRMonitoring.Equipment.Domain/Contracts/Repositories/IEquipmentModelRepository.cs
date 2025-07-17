using System;
using System.Threading.Tasks;
using System.Threading;
using Nomium.Core.Data.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.Domain.Models.EquipmentModel;

namespace RRMonitoring.Equipment.Domain.Contracts.Repositories;

public interface IEquipmentModelRepository : IRepository<EquipmentModel, Guid>
{
	Task<EquipmentModel> GetByName(string name, CancellationToken cancellationToken);

	Task<PagedList<EquipmentModel>> Search(SearchEquipmentModelsCriteria criteria, CancellationToken cancellationToken);
}