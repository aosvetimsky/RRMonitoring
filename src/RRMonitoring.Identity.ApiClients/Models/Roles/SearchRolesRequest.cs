using System;
using System.Diagnostics.CodeAnalysis;
using Nomium.Core.Models;

namespace RRMonitoring.Identity.ApiClients.Models.Roles;

public record SearchRolesRequest : RecordPagedRequest
{
	public string Keyword { get; set; }

	[SuppressMessage("Performance", "CA1819:Properties should not return arrays")] // TODO: Fix and break backward compatibility
	public Guid[] TenantIds { get; set; }
}
