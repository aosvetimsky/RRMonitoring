using Nomium.Core.Models;

namespace RRMonitoring.Mining.PublicModels.Pools;

public class SearchPoolsRequestDto : PagedRequest
{
	public string Keyword { get; set; }
}
