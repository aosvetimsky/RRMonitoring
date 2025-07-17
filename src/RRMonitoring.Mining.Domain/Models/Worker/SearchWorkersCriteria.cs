using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Mining.Domain.Models.Worker;

public class SearchWorkersCriteria : PagedRequest
{
	public string Keyword { get; set; }

	public IReadOnlyList<Guid> PoolIds { get; set; }

	public IReadOnlyList<byte> CoinIds { get; set; }

	public IReadOnlyList<byte> StatusIds { get; set; }
}
