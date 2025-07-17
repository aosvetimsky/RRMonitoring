using System;
using System.Collections.Generic;

namespace RRMonitoring.Equipment.PublicModels.Firmware;

public class UpdateFirmwareRequestDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Version { get; set; }

	public string Comment { get; set; }

	public IReadOnlyList<Guid> EquipmentModelIds { get; set; }
}
