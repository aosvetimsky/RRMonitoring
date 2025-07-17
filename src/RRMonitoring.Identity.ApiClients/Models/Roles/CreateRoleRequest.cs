using System;
using System.Collections.Generic;

namespace RRMonitoring.Identity.ApiClients.Models.Roles;

public class CreateRoleRequest
{
	public string Name { get; set; }

	public Guid? TenantId { get; set; }

	public IList<Guid> PermissionIds { get; set; }
}
