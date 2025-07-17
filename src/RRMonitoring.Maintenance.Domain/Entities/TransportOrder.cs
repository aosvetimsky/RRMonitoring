using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Maintenance.Domain.Entities;

public class TransportOrder : EntityBase<Guid>, IAuditableDateTime
{
	public string FromLocation { get; set; }

	public string ToLocation { get; set; }

	public Guid? ServiceCenterId { get; set; }
	public ServiceCenter ServiceCenter { get; set; }

	public decimal? TransportCost { get; set; }

	public byte StatusId { get; set; } // Planned, InTransit, Delivered, Canceled

	public DateTime OrderDate { get; set; }

	public DateTime? EstimatedDeliveryDate { get; set; }

	public DateTime? ActualDeliveryDate { get; set; }

	public ICollection<TransportOrderItem> Items { get; set; } = new List<TransportOrderItem>();

	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
}