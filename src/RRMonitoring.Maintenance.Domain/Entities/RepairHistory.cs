using System;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Maintenance.Domain.Entities;

public class RepairHistory : EntityBase<Guid>, IAuditableDateTime
{
	public Guid RepairRequestId { get; set; }
	public RepairRequest RepairRequest { get; set; }

	public DateTime ActionDate { get; set; }
	public byte ActionTypeId { get; set; } // Moved, PartReplaced, StatusChanged и т.д.

	public string OldValue { get; set; }
	public string NewValue { get; set; }
	public string Comment { get; set; }

	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
}