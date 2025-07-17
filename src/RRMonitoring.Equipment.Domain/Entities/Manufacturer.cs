using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Equipment.Domain.Entities;

public class Manufacturer : AuditableEntity
{
	public string Name { get; set; }

	public ICollection<EquipmentModel> EquipmentModels { get; set; } = new List<EquipmentModel>();
}
