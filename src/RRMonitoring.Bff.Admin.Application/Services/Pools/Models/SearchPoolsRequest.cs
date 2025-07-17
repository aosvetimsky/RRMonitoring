using Nomium.Core.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Pools.Models;

public class SearchPoolsRequest : PagedRequest
{
	public string Keyword { get; set; }
}
