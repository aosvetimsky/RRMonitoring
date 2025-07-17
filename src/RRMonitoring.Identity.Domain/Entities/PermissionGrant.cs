using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class PermissionGrant : AuditableEntity
{
	public Guid SourceUserId { get; set; }
	public User SourceUser { get; set; }

	public Guid DestinationUserId { get; set; }
	public User DestinationUser { get; set; }

	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }

	public string Reason { get; set; }

	public ICollection<PermissionGrantPermission> GrantedPermissions { get; set; }
}
