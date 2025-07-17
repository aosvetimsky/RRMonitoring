using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Colocation.Domain.Entities;

public class Rack : AuditableEntity
{
	public Guid ContainerId { get; set; }
	public Container Container { get; set; }

	public string Name { get; set; }

	public int Number { get; set; }

	public string Description { get; set; }

	// TODO: Do we really need this?
	public byte SocketTypeId { get; set; }
	public SocketType SocketType { get; set; }

	public ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();
}
