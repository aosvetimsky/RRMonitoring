using System;
using System.Collections.Generic;

namespace RRMonitoring.Bff.Admin.Application.Services.Pools.Models;

public class SearchPoolsResponseItem
{
	public Guid Id { get; init; }

	public string Name { get; init; }

	public IReadOnlyList<SearchPoolsCoinResponse> Coins { get; init; }
}

public class SearchPoolsCoinResponse
{
	public byte CoinId { get; init; }

	public string CoinName { get; init; }
}
