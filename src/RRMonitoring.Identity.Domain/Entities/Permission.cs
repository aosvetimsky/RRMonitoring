using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class Permission : EntityBase
{
	public string Name { get; set; }

	public string DisplayName { get; set; }

	public int GroupId { get; set; }
	public PermissionGroup Group { get; set; }

	public ICollection<RolePermission> RolePermissions { get; set; }
}
