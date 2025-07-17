using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Equipment.Domain.Entities;

public class Equipment : AuditableEntity
{
	public Guid ModelId { get; set; }
	public EquipmentModel Model { get; set; }

	public string SerialNumber { get; set; }

	public string IpAddress { get; set; }

	public string MacAddress { get; set; }

	public decimal CurrentHashrate { get; set; }

	public decimal CurrentPower { get; set; }

	public byte StatusId { get; set; }
	public EquipmentStatus Status { get; set; }

	public byte ModeId { get; set; }
	public EquipmentMode Mode { get; set; }

	public bool IsDeleted { get; set; }

	public ICollection<ScanResult> ScanResults { get; set; } = new List<ScanResult>();
}
