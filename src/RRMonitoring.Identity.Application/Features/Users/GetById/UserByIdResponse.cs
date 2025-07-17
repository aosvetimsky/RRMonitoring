using System;
using System.Collections.Generic;

namespace RRMonitoring.Identity.Application.Features.Users.GetById;

public class UserByIdResponse
{
	public Guid Id { get; set; }
	public int SerialNumber { get; set; }

	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string MiddleName { get; set; }

	public string Login { get; set; }
	public string Email { get; set; }
	public bool EmailConfirmed { get; set; }
	public string PhoneNumber { get; set; }
	public bool PhoneNumberConfirmed { get; set; }
	public string ExternalId { get; set; }

	public bool IsBlocked { get; set; }
	public DateTime? BlockedDate { get; set; }
	public DateTimeOffset? LockoutEndDate { get; set; }
	public DateTime? LastLoginDate { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }

	public bool IsAdmin { get; set; }
	public bool IsAgreementAcceptanceRequired { get; set; }

	public UserByIdStatusResponse Status { get; set; }
	public UserByIdTypeResponse Type { get; set; }
	public UserByIdExternalSourceResponse ExternalSource { get; set; }
	public List<UserByIdRoleResponse> Roles { get; set; }
	public List<UserByIdTenantResponse> Tenants { get; set; }
}

public class UserByIdStatusResponse
{
	public byte Id { get; set; }
	public string Name { get; set; }
}

public class UserByIdTypeResponse
{
	public byte Id { get; set; }
	public string Name { get; set; }
	public string Code { get; set; }
}

public class UserByIdRoleResponse
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Code { get; set; }
}

public class UserByIdTenantResponse
{
	public Guid Id { get; set; }
	public string Name { get; set; }
}

public class UserByIdExternalSourceResponse
{
	public byte Id { get; set; }

	public string Name { get; set; }

	public string Code { get; set; }
}
