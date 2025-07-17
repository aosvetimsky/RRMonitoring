using System;
using System.Collections.Generic;

namespace RRMonitoring.Identity.ApiClients.Models.Roles;

public class RoleResponse
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Code { get; set; }

	public bool IsSystem { get; set; }

	public RoleTenant Tenant { get; set; }

	public IList<Guid> PermissionIds { get; set; }
}

public class RoleTenant
{
	public Guid Id { get; set; }
	public string Name { get; set; }
}
