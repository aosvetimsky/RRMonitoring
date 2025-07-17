using System;

namespace RRMonitoring.Mining.PublicModels.Workers;

public class SearchWorkersResponseDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string DisplayName { get; set; }

	public string ClientName { get; set; }

	public string PoolName { get; set; }

	public SearchWorkersCoinResponseDto Coin { get; set; }

	public SearchWorkersStatusResponseDto Status { get; set; }
}

public class SearchWorkersCoinResponseDto
{
	public byte Id { get; init; }

	public string Name { get; init; }

	public string Ticker { get; init; }
}

public class SearchWorkersStatusResponseDto
{
	public byte Id { get; init; }

	public string Name { get; init; }
}
