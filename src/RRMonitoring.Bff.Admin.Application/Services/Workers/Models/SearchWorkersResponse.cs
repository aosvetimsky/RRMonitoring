using System;

namespace RRMonitoring.Bff.Admin.Application.Services.Workers.Models;

public class SearchWorkersResponse
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string DisplayName { get; set; }

	public string ClientName { get; set; }

	public string PoolName { get; set; }

	public int EquipmentQuantity { get; set; }

	public int EquipmentUnderRepairQuantity { get; set; }

	public int TotalHashrate { get; set; }

	public SearchWorkersCoinResponse Coin { get; set; }

	public SearchWorkersStatusResponse Status { get; set; }
}

public class SearchWorkersCoinResponse
{
	public byte Id { get; init; }

	public string Name { get; init; }

	public string Ticker { get; init; }
}

public class SearchWorkersStatusResponse
{
	public byte Id { get; init; }

	public string Name { get; init; }
}
