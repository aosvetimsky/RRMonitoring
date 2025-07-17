using System;
using IdentityServer4.EntityFramework.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class ScopePermission
{
	public int ScopeId { get; set; }
	public ApiScope Scope { get; set; }

	public Guid PermissionId { get; set; }
	public Permission Permission { get; set; }
}
