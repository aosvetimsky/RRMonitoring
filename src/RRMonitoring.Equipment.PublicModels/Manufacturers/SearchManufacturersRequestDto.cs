using Nomium.Core.Models;

namespace RRMonitoring.Equipment.PublicModels.Manufacturers;

public class SearchManufacturersRequestDto : PagedRequest
{
	public string Keyword { get; set; }
}
