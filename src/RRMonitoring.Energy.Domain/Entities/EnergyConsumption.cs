using System;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Energy.Domain.Entities;

public class EnergyConsumption : EntityBase<Guid>, IAuditableDateTime
{
	public decimal ConsumedValue { get; set; }

	public DateTime IntervalStart { get; set; }

	public DateTime IntervalEnd { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime? UpdatedDate { get; set; }

	public Guid UsagePointId { get; set; }

	public UsagePoint UsagePoint { get; set; }
}
