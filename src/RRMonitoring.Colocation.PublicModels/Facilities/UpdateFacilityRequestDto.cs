using System;
using System.Collections.Generic;

namespace RRMonitoring.Colocation.PublicModels.Facilities;

public class UpdateFacilityRequestDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public int PowerCapacity { get; set; }

	public string Subnet { get; set; }

	public Guid ManagerId { get; set; }

	public Guid DeputyManagerId { get; set; }

	public IReadOnlyList<Guid> TechnicianIds { get; set; }
}
