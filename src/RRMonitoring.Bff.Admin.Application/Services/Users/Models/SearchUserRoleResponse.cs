using System;
using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Bff.Admin.Application.Services.Users.Models;

public class SearchUserRoleResponse
{
	[Required]
	public Guid Id { get; set; }

	[Required]
	public string Name { get; set; }
}
