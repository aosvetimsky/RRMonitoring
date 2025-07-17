using System;
using System.Collections.Generic;

namespace RRMonitoring.Identity.ApiClients.Models.Users;

public class SearchUsersResponseItem
{
	public Guid Id { get; set; }
	public string Login { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public List<SearchUserRoleResponse> Roles { get; set; }
	public SearchUserStatusResponse Status { get; set; }
	public DateTimeOffset? LockoutEndDate { get; set; }
	public DateTime? LastLoginDate { get; set; }
	public DateTime? BlockedDate { get; set; }
	public string BlockReason { get; set; }
	public string BlockedByAdmin { get; set; }
	public bool TwoFactorEnabled { get; set; }
}

public class SearchUserRoleResponse
{
	public Guid Id { get; set; }
	public string Name { get; set; }
}

public class SearchUserStatusResponse
{
	public byte Id { get; set; }
	public string Name { get; set; }
}
