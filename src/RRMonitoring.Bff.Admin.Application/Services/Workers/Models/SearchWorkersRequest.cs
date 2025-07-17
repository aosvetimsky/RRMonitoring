using System.Collections.Generic;
using System;
using Nomium.Core.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Workers.Models;

public class SearchWorkersRequest : PagedRequest
{
	public string Keyword { get; set; }

	public IReadOnlyList<Guid> PoolIds { get; set; }

	public IReadOnlyList<byte> CoinIds { get; set; }

	public IReadOnlyList<byte> StatusIds { get; set; }
}
