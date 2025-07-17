using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Energy.Domain.Entities;

public class UsagePoint : EntityBase<Guid>, IAuditableDateTime
{
	public Guid? ContainerId { get; set; }

	public Guid? FacilityId { get; set; }

	public bool IsActive { get; set; }

	public Guid TransformerId { get; set; }

	public Transformer Transformer { get; set; }

	public ICollection<MeterReading> MeterReadings { get; set; } = new List<MeterReading>();

	public ICollection<EnergyConsumption> EnergyConsumptions { get; set; } = new List<EnergyConsumption>();

	public DateTime CreatedDate { get; set; }

	public DateTime? UpdatedDate { get; set; }
}
