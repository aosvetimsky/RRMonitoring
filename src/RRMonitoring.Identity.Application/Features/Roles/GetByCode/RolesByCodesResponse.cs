using System;
using System.Collections.Generic;

namespace RRMonitoring.Identity.Application.Features.Roles.GetByCode;

public class RolesByCodesResponse
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public bool IsSystem { get; set; }

	public string Code { get; set; }

	public InternalRoleTenant Tenant { get; set; }

	public List<Guid> PermissionIds { get; set; }
}

public class InternalRoleTenant
{
	public Guid Id { get; set; }

	public string Name { get; set; }
}
