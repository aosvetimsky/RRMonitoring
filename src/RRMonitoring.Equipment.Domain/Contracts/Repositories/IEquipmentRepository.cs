using System;
using Nomium.Core.Data.Repositories;

namespace RRMonitoring.Equipment.Domain.Contracts.Repositories;

public interface IEquipmentRepository : IRepository<Entities.Equipment, Guid>;
