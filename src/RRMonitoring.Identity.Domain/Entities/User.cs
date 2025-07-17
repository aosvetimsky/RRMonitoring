using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class User : IdentityUser<Guid>, IEntity<Guid>, IAuditableDateTime
{
	public int SerialNumber { get; set; }

	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string MiddleName { get; set; }
	public string TelegramLogin { get; set; }
	public string UnconfirmedEmail { get; set; }
	public string ExternalId { get; set; }

	public bool IsBlocked { get; set; }
	public DateTime? BlockedDate { get; set; }
	public string BlockReason { get; set; }
	public Guid? BlockedBy { get; set; }
	public User BlockedUser { get; set; }

	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public DateTime? LastLoginDate { get; set; }

	public bool IsAdmin { get; set; }

	public byte StatusId { get; set; }
	public UserStatus Status { get; set; }

	public bool IsAuthenticatorEnabled { get; set; }

	public byte? TypeId { get; set; }
	public UserType Type { get; set; }

	public bool IsAgreementAcceptanceRequired { get; set; }
	public DateTime? AgreementAcceptedDate { get; set; }

	public byte? ExternalSourceId { get; set; }
	public ExternalSource ExternalSource { get; set; }

	public ICollection<UserRole> UserRoles { get; set; }

	public ICollection<TenantUser> UserTenants { get; set; }

	public ICollection<UsedUserPassword> UsedPasswords { get; set; }

	public ICollection<UserEvent> UserEvents { get; set; }

	public string FullName => $"{LastName} {FirstName} {MiddleName}";

	public bool IsExternal => ExternalSourceId.HasValue;
}
