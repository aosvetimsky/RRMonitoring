using System;
using System.Collections.Generic;

namespace RRMonitoring.Equipment.PublicModels.EquipmentModels;

public class EquipmentModelByIdResponseDto
{
	public Guid Id { get; init; }

	public string Name { get; init; }

	public Guid ManufacturerId { get; set; }

	public byte HashrateUnitId { get; set; }

	public decimal NominalHashrate { get; set; }

	public int NominalPower { get; set; }

	public int MaxMotherBoardTemperature { get; set; }

	public int MaxProcessorTemperature { get; set; }

	public IReadOnlyList<byte> CoinIds { get; set; }
}
