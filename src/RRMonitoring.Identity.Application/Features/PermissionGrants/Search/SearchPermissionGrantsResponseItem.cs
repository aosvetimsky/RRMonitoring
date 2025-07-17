using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Identity.Application.Features.PermissionGrants.Search;

public class SearchPermissionGrantsResponseItem
{
	public Guid Id { get; set; }
	public SearchPermissionGrantsUserResponse SourceUser { get; set; }
	public SearchPermissionGrantsUserResponse DestinationUser { get; set; }
	public DateTimePeriod GrantDates { get; set; }
	public IList<SearchPermissionGrantPermissionResponseItem> Permissions { get; set; }
	public string Reason { get; set; }
}

public class SearchPermissionGrantsUserResponse
{
	public Guid Id { get; set; }
	public string FullName { get; set; }
}

public class SearchPermissionGrantPermissionResponseItem
{
	public Guid Id { get; set; }
	public string DisplayName { get; set; }
}
