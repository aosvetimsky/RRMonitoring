using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Colocation.Domain.Entities;

public class Shelf : AuditableEntity
{
	public Guid RackId { get; set; }
	public Rack Rack { get; set; }

	public string Name { get; set; }

	public int Number { get; set; }

	public byte SocketTypeId { get; set; }
	public SocketType SocketType { get; set; }

	public ICollection<Place> Places { get; set; } = new List<Place>();
}
