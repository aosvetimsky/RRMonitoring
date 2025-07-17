using System;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Maintenance.Domain.Entities;

public class TransportOrderItem : EntityBase<Guid>
{
	public Guid TransportOrderId { get; set; }
	public TransportOrder TransportOrder { get; set; }

	public Guid EquipmentId { get; set; }

	public int Quantity { get; set; }

	public decimal? ItemTransportCost { get; set; }
}