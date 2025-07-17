using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Bff.Admin.Application.Services.Firmware.Models;

public class UpdateFirmwareRequest
{
	[Required]
	public Guid Id { get; set; }

	[Required]
	public string Name { get; set; }

	[Required]
	public string Version { get; set; }

	public string Comment { get; set; }

	[Required]
	public IReadOnlyList<Guid> EquipmentModelIds { get; set; }
}
