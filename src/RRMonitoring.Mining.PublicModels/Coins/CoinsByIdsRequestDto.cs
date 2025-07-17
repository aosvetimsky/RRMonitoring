using System;
using System.Collections.Generic;

namespace RRMonitoring.Mining.PublicModels.Coins;

public class CoinsByIdsRequestDto
{
	public IReadOnlyList<Guid> Ids { get; set; }
}
