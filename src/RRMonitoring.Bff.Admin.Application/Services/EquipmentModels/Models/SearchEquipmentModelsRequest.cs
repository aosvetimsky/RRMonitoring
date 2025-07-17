using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.EquipmentModels.Models;

public class SearchEquipmentModelsRequest : PagedRequest
{
	public string Keyword { get; set; }

	public IReadOnlyList<Guid> ManufacturerIds { get; set; }
}
