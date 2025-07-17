using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Maintenance.Domain.Entities;

public class RepairRequest : EntityBase<Guid>, IAuditableDateTime
{
	public Guid EquipmentId { get; set; }

	public byte StatusId { get; set; }

	public DateTime OpenedDate { get; set; }

	public DateTime? ClosedDate { get; set; }

	public Guid CreatedByUserId { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime? UpdatedDate { get; set; }

	public Guid? WarrantyOrderId { get; set; }

	public WarrantyOrder WarrantyOrder { get; set; }

	public ICollection<RepairHistory> HistoryRecords { get; set; } = new List<RepairHistory>();
}
