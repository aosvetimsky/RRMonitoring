using Nomium.Core.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Equipment.Models;

public class SearchEquipmentManufacturersRequest : PagedRequest
{
	public string Keyword { get; set; }
}
