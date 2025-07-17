using System;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class UsedUserPassword : EntityBase
{
	public Guid UserId { get; set; }
	public User User { get; set; }

	public string PasswordHash { get; set; }

	public DateTime CreatedDate { get; set; }
}
