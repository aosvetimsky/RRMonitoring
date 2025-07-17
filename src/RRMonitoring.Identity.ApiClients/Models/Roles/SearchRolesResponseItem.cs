using System;

namespace RRMonitoring.Identity.ApiClients.Models.Roles;

public class SearchRolesResponseItem
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public bool IsSystem { get; set; }
}
