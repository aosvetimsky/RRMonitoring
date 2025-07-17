using System;
using System.Collections.Generic;

namespace RRMonitoring.Identity.ApiClients.Models.Users;

public record UpdateUserRequest
{
	public Guid Id { get; set; }
	public string Login { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string MiddleName { get; set; }
	public string Email { get; set; }
	public bool? EmailConfirmed { get; set; }
	public string PhoneNumber { get; set; }
	public bool? PhoneNumberConfirmed { get; set; }
	public string ExternalId { get; set; }
	public byte? UserTypeId { get; set; }
	public byte? ExternalSourceId { get; set; }
	public bool IsAgreementAcceptanceRequired { get; set; }
	public ICollection<Guid> RoleIds { get; set; }
	public ICollection<Guid> TenantIds { get; set; }
}
