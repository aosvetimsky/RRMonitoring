using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Mining.Domain.Entities;

public class Client : EntityBase<Guid>, IAuditableDateTime
{
	public string Name { get; set; }

	public string ExternalId { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime? UpdatedDate { get; set; }

	public ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
