using Nomium.Core.Models;

namespace RRMonitoring.Colocation.Domain.Models.Facility;

public class SearchFacilitiesCriteria : PagedRequest
{
	public bool? IsArchived { get; set; }
}
