using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Identity.Domain.Models;

public class SearchPermissionGrantsPagedCriteria : PagedRequest, ISearchPermissionGrantsCriteria
{
	public IList<Guid> SourceUserIds { get; set; }
	public IList<Guid> DestinationUserIds { get; set; }
	public DateTimePeriod? GrantDates { get; set; }
	public IList<Guid> PermissionIds { get; set; }
}
