using System;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Colocation.Domain.Entities;

public class FacilityTechnician
{
	public Guid UserId { get; set; }

	public Guid FacilityId { get; set; }
	public Facility Facility { get; set; }
}
