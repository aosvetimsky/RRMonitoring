using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Colocation.Domain.Entities;

public class Facility : AuditableEntity
{
	public string Name { get; set; }

	public int PowerCapacity { get; set; }

	public bool IsArchived { get; set; }

	public Guid ManagerId { get; set; }

	public Guid DeputyManagerId { get; set; }

	public string Subnet { get; set; }

	public ICollection<Container> Containers { get; set; } = new List<Container>();

	public ICollection<FacilityTechnician> Technicians { get; set; } = new List<FacilityTechnician>();
}
