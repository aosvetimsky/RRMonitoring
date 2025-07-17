using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class Role : IdentityRole<Guid>, IEntity<Guid>
{
	public Guid? TenantId { get; set; }

	public Tenant Tenant { get; set; }

	public string Code { get; set; }

	public bool IsSystem { get; set; }

	public ICollection<UserRole> UserRoles { get; set; }

	public ICollection<RolePermission> RolePermissions { get; set; }
}
