using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Bff.Admin.Application.Services.Users.Models;

public class SearchUsersResponseItem
{
	[Required]
	public Guid Id { get; set; }

	[Required]
	public string Login { get; set; }

	[Required]
	public bool TwoFactorEnabled { get; set; }

	[Required]
	public SearchUserStatusResponse Status { get; set; }

	public string Email { get; set; }

	public string PhoneNumber { get; set; }

	public string BlockReason { get; set; }

	public string BlockedByAdmin { get; set; }

	public DateTimeOffset? LockoutEndDate { get; set; }

	public DateTime? LastLoginDate { get; set; }

	public DateTime? BlockedDate { get; set; }

	public List<SearchUserRoleResponse> Roles { get; set; }
}
