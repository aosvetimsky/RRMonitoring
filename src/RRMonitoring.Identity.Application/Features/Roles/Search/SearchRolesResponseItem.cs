using System;
using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Identity.Application.Features.Roles.Search;

public class SearchRolesResponseItem
{
	[Required]
	public Guid Id { get; set; }

	[Required]
	public string Name { get; set; }

	[Required]
	public bool IsSystem { get; set; }
}
