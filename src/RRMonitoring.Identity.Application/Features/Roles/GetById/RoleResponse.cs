using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Identity.Application.Features.Roles.GetById;

public class RoleResponse
{
	[Required]
	public Guid Id { get; set; }

	[Required]
	public string Name { get; set; }

	[Required]
	public string Code { get; set; }

	[Required]
	public bool IsSystem { get; set; }

	public RoleTenant Tenant { get; set; }

	public IList<Guid> PermissionIds { get; set; }
}

public class RoleTenant
{
	public Guid Id { get; set; }
	public string Name { get; set; }
}
