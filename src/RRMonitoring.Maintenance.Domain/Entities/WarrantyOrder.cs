using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Maintenance.Domain.Entities;

public class WarrantyOrder : EntityBase<Guid>, IAuditableDateTime
{
	public string OrderNumber { get; set; }

	public byte PaymentTypeId { get; set; }

	public decimal? TotalCost { get; set; }

	public ICollection<RepairRequest> RepairRequests { get; set; } = new List<RepairRequest>();

	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
}