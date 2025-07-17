using Nomium.Core.Models;

namespace RRMonitoring.Mining.Domain.Models.Pool;

public class SearchPoolsCriteria : PagedRequest
{
	public string Keyword { get; set; }
}
