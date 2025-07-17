using System;
using System.Collections.Generic;

namespace RRMonitoring.Bff.Admin.Application.Services.Facilities.Models;

public class FacilityByIdResponse
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public int PowerCapacity { get; set; }

	public string Subnet { get; set; }

	public Guid ManagerId { get; set; }

	public Guid DeputyManagerId { get; set; }

	public IReadOnlyList<Guid> TechnicianIds { get; set; }
}
