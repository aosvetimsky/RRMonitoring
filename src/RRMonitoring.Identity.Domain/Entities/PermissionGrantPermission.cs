using System;

namespace RRMonitoring.Identity.Domain.Entities;

public class PermissionGrantPermission
{
	public Guid PermissionGrantId { get; set; }
	public PermissionGrant PermissionGrant { get; set; }

	public Guid PermissionId { get; set; }
	public Permission Permission { get; set; }
}
