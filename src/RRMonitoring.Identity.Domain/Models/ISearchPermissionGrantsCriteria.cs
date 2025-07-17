using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Identity.Domain.Models;

public interface ISearchPermissionGrantsCriteria
{
	IList<Guid> SourceUserIds { get; set; }
	IList<Guid> DestinationUserIds { get; set; }
	DateTimePeriod? GrantDates { get; set; }
	IList<Guid> PermissionIds { get; set; }
}
