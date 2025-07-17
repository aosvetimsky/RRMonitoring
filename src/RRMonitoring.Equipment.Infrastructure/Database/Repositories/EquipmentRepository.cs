using System;
using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;

namespace RRMonitoring.Equipment.Infrastructure.Database.Repositories;

public class EquipmentRepository(EquipmentContext context)
	: RepositoryBase<Domain.Entities.Equipment, Guid>(context), IEquipmentRepository;
