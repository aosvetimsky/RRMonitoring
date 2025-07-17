using Nomium.Core.Data.Repositories;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Domain.Contracts.Repositories;

public interface IHashrateUnitRepository : IDictionaryRepository<HashrateUnit, byte>;
