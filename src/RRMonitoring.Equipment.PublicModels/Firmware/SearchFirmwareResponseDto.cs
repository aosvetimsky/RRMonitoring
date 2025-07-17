using System;
using System.Collections.Generic;

namespace RRMonitoring.Equipment.PublicModels.Firmware;

public class SearchFirmwareResponseDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Version { get; set; }

	public string Comment { get; set; }

	public IReadOnlyList<SearchFirmwareEquipmentModelResponseDto> EquipmentModels { get; set; }

	public Guid CreatedBy { get; set; }

	public DateTime CreatedDate { get; set; }
}

public class SearchFirmwareEquipmentModelResponseDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }
}
