using System;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Maintenance.Domain.Entities;

public class EquipmentLabel : EntityBase<Guid>, IAuditableDateTime
{
	public Guid EquipmentId { get; set; }

	public string LabelCode { get; set; }

	public string LabelFilePath { get; set; }

	public bool IsPrinted { get; set; }

	public Guid? PrintedByUserId { get; set; }

	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
}