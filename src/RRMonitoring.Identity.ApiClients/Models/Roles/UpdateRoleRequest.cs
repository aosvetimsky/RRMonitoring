using System;
using System.Collections.Generic;

namespace RRMonitoring.Identity.ApiClients.Models.Roles;

public class UpdateRoleRequest
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public Guid? TenantId { get; set; }

	public IList<Guid> PermissionIds { get; set; }
}
