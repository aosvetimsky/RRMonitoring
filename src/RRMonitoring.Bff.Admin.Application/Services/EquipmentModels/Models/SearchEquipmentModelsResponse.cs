using System;
using System.Collections.Generic;

namespace RRMonitoring.Bff.Admin.Application.Services.EquipmentModels.Models;

public class SearchEquipmentModelsResponse
{
	public Guid Id { get; init; }

	public string Name { get; init; }

	public string ManufacturerName { get; set; }

	public string Hashrate { get; set; }

	public int NominalPower { get; set; }

	public IReadOnlyList<SearchEquipmentModelCoinsResponse> Coins { get; set; }
}

public class SearchEquipmentModelCoinsResponse
{
	public byte Id { get; set; }

	public string Name { get; set; }

	public string Ticker { get; set; }
}
