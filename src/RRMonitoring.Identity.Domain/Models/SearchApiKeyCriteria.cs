using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Identity.Domain.Models;

public class SearchApiKeyCriteria : PagedRequest
{
	public Guid OwnerId { get; set; }

	public List<byte> TypeIds { get; set; }
}
