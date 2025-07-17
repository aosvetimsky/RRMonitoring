using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Colocation.Domain.Entities;

public class Container : AuditableEntity
{
	public int Number { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public Guid FacilityId { get; set; }
	public Facility Facility { get; set; }

	public byte SocketTypeId { get; set; }
	public SocketType SocketType { get; set; }

	public ICollection<Rack> Racks { get; set; } = new List<Rack>();
}
