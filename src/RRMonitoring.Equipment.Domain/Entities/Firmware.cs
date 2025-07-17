using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Equipment.Domain.Entities;

public class Firmware : AuditableEntity
{
	public string Name { get; set; }

	public string Version { get; set; }

	public string Comment { get; set; }

	public string OriginFileName { get; set; }

	public ICollection<FirmwareEquipmentModel> FirmwareEquipmentModels { get; set; } = new List<FirmwareEquipmentModel>();
}
