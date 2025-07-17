using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class PermissionGroup : EntityBase<int>
{
	public string Name { get; set; }

	public ICollection<Permission> Permissions { get; set; }
}
