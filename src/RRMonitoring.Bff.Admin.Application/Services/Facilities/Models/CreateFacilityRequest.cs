using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Bff.Admin.Application.Services.Facilities.Models;

public class CreateFacilityRequest
{
	[Required]
	public string Name { get; set; }

	[Required]
	public int PowerCapacity { get; set; }

	public string Subnet { get; set; }

	[Required]
	public Guid ManagerId { get; set; }

	[Required]
	public Guid DeputyManagerId { get; set; }

	public IReadOnlyList<Guid> TechnicianIds { get; set; }
}
