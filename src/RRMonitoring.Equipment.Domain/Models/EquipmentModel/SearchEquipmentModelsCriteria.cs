using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Equipment.Domain.Models.EquipmentModel;

public class SearchEquipmentModelsCriteria : PagedRequest
{
	public string Keyword { get; set; }

	public IReadOnlyList<Guid> ManufacturerIds { get; set; }
}
