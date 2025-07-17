using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Nomium.Core.FileStorage.Attributes;
using RRMonitoring.Bff.Admin.Application.Constants;

namespace RRMonitoring.Bff.Admin.Application.Services.Firmware.Models;

public class CreateFirmwareRequest
{
	[Required]
	public string Name { get; set; }

	[Required]
	public string Version { get; set; }

	public string Comment { get; set; }

	[Required]
	public IReadOnlyList<Guid> EquipmentModelIds { get; set; }

	[Required]
	[MaxFileSize(500)]
	[AllowedFileExtensions(KnownFileExtensions.Bin, KnownFileExtensions.Zip, KnownFileExtensions.Bmu)]
	public IFormFile File { get; set; }
}
