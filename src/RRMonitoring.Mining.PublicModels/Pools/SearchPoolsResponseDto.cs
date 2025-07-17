using System;
using System.Collections.Generic;

namespace RRMonitoring.Mining.PublicModels.Pools;

public class SearchPoolsResponseDto
{
	public Guid Id { get; init; }

	public string Name { get; init; }

	public IReadOnlyList<SearchPoolsCoinResponseDto> Coins { get; init; }
}

public class SearchPoolsCoinResponseDto
{
	public byte CoinId { get; init; }

	public string CoinName { get; init; }
}
