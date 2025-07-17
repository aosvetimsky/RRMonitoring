using System;
using System.Collections.Generic;

namespace RRMonitoring.Bff.Admin.Application.Services.Firmware.Models;

public class SearchFirmwareResponse
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Version { get; set; }

	public string Comment { get; set; }

	public IReadOnlyList<SearchFirmwareEquipmentModelResponse> EquipmentModels { get; set; }

	public Guid CreatedBy { get; set; }

	public string CreatedByName { get; set; }

	public DateTime CreatedDate { get; set; }
}

public class SearchFirmwareEquipmentModelResponse
{
	public Guid Id { get; set; }

	public string Name { get; set; }
}
