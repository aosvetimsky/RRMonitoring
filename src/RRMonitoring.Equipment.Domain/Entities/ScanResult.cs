using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Equipment.Domain.Entities;

public class ScanResult : AuditableEntity
{
	public Guid ScanId { get; set; }
	public Scan Scan { get; set; }

	public string IpAddress { get; set; }

	public string MacAddress { get; set; }

	public string DetectedModel { get; set; }

	public string FirmwareVersion { get; set; }

	public Guid EquipmentId { get; set; }
	public Equipment Equipment { get; set; }

	public bool IsNewEquipment { get; set; }
}
