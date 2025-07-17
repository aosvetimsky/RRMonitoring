using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Energy.Domain.Entities;

public class Transformer : EntityBase<Guid>, IAuditableDateTime
{
	public string ExternalId { get; set; }

	public string Description { get; set; }

	public ICollection<UsagePoint> UsagePoints { get; set; } = new List<UsagePoint>();

	public DateTime CreatedDate { get; set; }

	public DateTime? UpdatedDate { get; set; }
}
