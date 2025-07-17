using Nomium.Core.Models;

namespace RRMonitoring.Equipment.Domain.Models.Manufacturer;

public class SearchManufacturersCriteria : PagedRequest
{
	public string Keyword { get; set; }
}
