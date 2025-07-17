using System;
using System.Collections.Generic;

namespace RRMonitoring.Equipment.PublicModels.EquipmentModels;

public class SearchEquipmentModelsResponseDto
{
	public Guid Id { get; init; }

	public string Name { get; init; }

	public string ManufacturerName { get; init; }

	public string HashrateUnitName { get; init; }

	public decimal NominalHashrate { get; init; }

	public int NominalPower { get; init; }

	public IReadOnlyList<byte> CoinIds { get; set; }
}
