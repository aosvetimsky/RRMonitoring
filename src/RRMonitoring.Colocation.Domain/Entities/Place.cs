using System;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Colocation.Domain.Entities;

public class Place : AuditableEntity
{
	public Guid ShelfId { get; set; }
	public Shelf Shelf { get; set; }

	public int Number { get; set; }

	public byte SocketTypeId { get; set; }
	public SocketType SocketType { get; set; }

	public Guid? EquipmentId { get; set; }
}
