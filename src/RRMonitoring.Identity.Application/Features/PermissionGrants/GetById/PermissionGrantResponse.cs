using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Identity.Application.Features.PermissionGrants.GetById;

public class PermissionGrantResponse
{
	public Guid Id { get; set; }
	public PermissionGrantUserResponse SourceUser { get; set; }
	public PermissionGrantUserResponse DestinationUser { get; set; }
	public DateTimePeriod GrantDates { get; set; }
	public IList<Guid> PermissionIds { get; set; }
	public string Reason { get; set; }
}

public class PermissionGrantUserResponse
{
	public Guid Id { get; set; }
	public string FullName { get; set; }
}

