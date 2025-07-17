using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Users.Models;

public class SearchUsersRequest : PagedRequest
{
	public string Keyword { get; set; }
	public List<Guid> RoleIds { get; set; }
	public List<int> StatusIds { get; set; }
}
