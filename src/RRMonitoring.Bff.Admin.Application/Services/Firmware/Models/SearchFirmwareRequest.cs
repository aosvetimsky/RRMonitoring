using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Firmware.Models;

public class SearchFirmwareRequest : PagedRequest
{
	public string Keyword { get; set; }

	public IReadOnlyList<Guid> EquipmentModelIds { get; set; }
}
