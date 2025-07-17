using System;

namespace RRMonitoring.Bff.Admin.Application.Services.Users.Models;

public class UpdateUserRoleRequest
{
	public Guid UserId { get; set; }

	public Guid RoleId { get; set; }
}
