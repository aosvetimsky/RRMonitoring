using System;
using System.Collections.Generic;
using Nomium.Core.Models;

namespace RRMonitoring.Identity.ApiClients.Models.Users;

public record SearchUsersRequest : RecordPagedRequest
{
	public string Keyword { get; set; }

	public bool CanSeeSensitiveData { get; set; }

	public List<Guid> RoleIds { get; set; }

	public List<int> StatusIds { get; set; }
}
