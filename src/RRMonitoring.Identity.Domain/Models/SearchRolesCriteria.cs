using System;
using System.Diagnostics.CodeAnalysis;
using Nomium.Core.Models;

namespace RRMonitoring.Identity.Domain.Models;

public class SearchRolesCriteria : PagedRequest
{
	public string Keyword { get; set; }

	[SuppressMessage("Performance", "CA1819:Properties should not return arrays")] // TODO: Fix and break backward compatibility
	public Guid[] TenantIds { get; set; }
}
