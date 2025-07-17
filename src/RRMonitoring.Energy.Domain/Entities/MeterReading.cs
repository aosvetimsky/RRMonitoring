using System;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Energy.Domain.Entities;

public class MeterReading : EntityBase<Guid>, IAuditableDateTime
{
	public Guid UsagePointId { get; set; }
	public UsagePoint UsagePoint { get; set; }

	public DateTime ReadingDateTime { get; set; }
	public decimal Value { get; set; }

	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
}