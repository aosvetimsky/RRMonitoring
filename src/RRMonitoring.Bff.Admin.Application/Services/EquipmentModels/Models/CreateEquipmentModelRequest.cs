using System;
using System.Collections.Generic;

namespace RRMonitoring.Bff.Admin.Application.Services.EquipmentModels.Models;

public class CreateEquipmentModelRequest
{
	public string Name { get; set; }

	public Guid ManufacturerId { get; set; }

	public byte HashrateUnitId { get; set; }

	public decimal NominalHashrate { get; set; }

	public int NominalPower { get; set; }

	public int MaxMotherBoardTemperature { get; set; }

	public int MaxProcessorTemperature { get; set; }

	public IReadOnlyList<byte> CoinIds { get; set; }
}
