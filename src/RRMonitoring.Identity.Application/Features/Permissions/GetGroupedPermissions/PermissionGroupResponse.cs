using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Identity.Application.Features.Permissions.GetGroupedPermissions;

public class PermissionGroupResponse
{
	[Required]
	public int Id { get; set; }

	[Required]
	public string Name { get; set; }

	[Required]
	public IList<PermissionResponse> Permissions { get; set; }
}

public class PermissionResponse
{
	[Required]
	public Guid Id { get; set; }

	[Required]
	public string Name { get; set; }
}
