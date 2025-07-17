using System;
using System.Collections.Generic;

namespace RRMonitoring.Bff.Admin.Application.Services.Firmware.Models;

public class FirmwareByIdResponse
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Version { get; set; }

	public string Comment { get; set; }

	public IReadOnlyList<Guid> EquipmentModelIds { get; set; }
}
