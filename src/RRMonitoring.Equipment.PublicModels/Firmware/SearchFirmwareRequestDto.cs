using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Equipment.PublicModels.Firmware;

public class SearchFirmwareRequestDto : PagedRequest
{
	public string Keyword { get; set; }

	public IReadOnlyList<Guid> EquipmentModelIds { get; set; }
}
