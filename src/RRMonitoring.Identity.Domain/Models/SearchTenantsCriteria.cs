using Nomium.Core.Models;

namespace RRMonitoring.Identity.Domain.Models;

public class SearchTenantsCriteria : PagedRequest
{
	public string Keyword { get; set; }
}
