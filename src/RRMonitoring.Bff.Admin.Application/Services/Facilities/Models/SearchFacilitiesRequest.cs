using Nomium.Core.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Facilities.Models;

public class SearchFacilitiesRequest : PagedRequest
{
	public bool? IsArchived { get; set; }
}
