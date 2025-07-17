using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Infrastructure.Database.Repositories;

internal class HashrateUnitRepository(EquipmentContext context)
  : DictionaryRepository<HashrateUnit, byte>(context), IHashrateUnitRepository
{
}
