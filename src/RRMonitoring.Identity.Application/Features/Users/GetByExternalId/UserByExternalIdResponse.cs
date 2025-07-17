using System;

namespace RRMonitoring.Identity.Application.Features.Users.GetByExternalId;

public class UserByExternalIdResponse
{
	public Guid Id { get; set; }

	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string MiddleName { get; set; }

	public string Login { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public string ExternalId { get; set; }
	public bool IsBlocked { get; set; }

	public UserByExternalIdStatusResponse Status { get; set; }
	public UserByExternalIdTypeResponse Type { get; set; }
}

public class UserByExternalIdStatusResponse
{
	public byte Id { get; set; }
	public string Name { get; set; }
}

public class UserByExternalIdTypeResponse
{
	public byte Id { get; set; }
	public string Name { get; set; }
	public string Code { get; set; }
}
