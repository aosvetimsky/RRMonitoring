using System;
using System.Collections.Generic;

namespace RRMonitoring.Identity.ApiClients.Models.Users;

public record CreateUserRequest
{
	public string Login { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string MiddleName { get; set; }
	public string Email { get; set; }
	public string PhoneNumber { get; set; }
	public byte? UserTypeId { get; set; }
	public bool IsAgreementAcceptanceRequired { get; set; }
	public bool IsResetPasswordLinkSendingRequired { get; set; }
	public ICollection<Guid> RoleIds { get; set; }
	public ICollection<Guid> TenantIds { get; set; }
}
