using Nomium.Core.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Manufacturers.Models;

public class SearchManufacturersRequest : PagedRequest
{
	public string Keyword { get; set; }
}
