using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Equipment.PublicModels.EquipmentModels;

public class SearchEquipmentModelsRequestDto : PagedRequest
{
	public string Keyword { get; set; }

	public IReadOnlyList<Guid> ManufacturerIds { get; set; }
}
