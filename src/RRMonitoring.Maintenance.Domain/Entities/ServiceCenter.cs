using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Maintenance.Domain.Entities;

public class ServiceCenter : EntityBase<Guid>, IAuditableDateTime
{
	public string Name { get; set; }

	public string Address { get; set; }

	public string ContactInfo { get; set; }

	public ICollection<TransportOrder> TransportOrders { get; set; } = new List<TransportOrder>();

	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
}