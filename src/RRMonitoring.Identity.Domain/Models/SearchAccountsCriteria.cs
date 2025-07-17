using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Identity.Domain.Models;

public class SearchAccountsCriteria : PagedRequest
{
	public Guid UserId { get; set; }

	public Guid? PriorityAccountId { get; set; }

	public List<Guid> AvailableIds { get; set; }
}
