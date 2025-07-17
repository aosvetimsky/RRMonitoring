using Nomium.Core.Models;

namespace RRMonitoring.Colocation.PublicModels.Facilities;

public class SearchFacilitiesRequestDto : PagedRequest
{
	public bool? IsArchived { get; set; }
}
