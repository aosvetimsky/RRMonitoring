using System.Collections.Generic;
using System;
using Nomium.Core.Models;

namespace RRMonitoring.Equipment.Domain.Models.Firmware;

public class SearchFirmwareCriteria : PagedRequest
{
	public string Keyword { get; set; }

	public IReadOnlyList<Guid> EquipmentModelIds { get; set; }
}
