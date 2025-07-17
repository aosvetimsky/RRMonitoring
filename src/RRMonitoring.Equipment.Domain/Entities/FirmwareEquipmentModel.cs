using System;

namespace RRMonitoring.Equipment.Domain.Entities;

public class FirmwareEquipmentModel
{
	public Guid FirmwareId { get; set; }
	public Firmware Firmware { get; set; }

	public Guid EquipmentModelId { get; set; }
	public EquipmentModel EquipmentModel { get; set; }
}
