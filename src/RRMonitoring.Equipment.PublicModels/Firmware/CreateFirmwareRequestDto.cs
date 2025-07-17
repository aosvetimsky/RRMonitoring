using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace RRMonitoring.Equipment.PublicModels.Firmware;

public class CreateFirmwareRequestDto
{
	public string Name { get; set; }

	public string Version { get; set; }

	public string Comment { get; set; }

	public IEnumerable<Guid> EquipmentModelIds { get; set; }

	public IFormFile File { get; set; }
}
