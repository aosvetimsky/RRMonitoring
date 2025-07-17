using System;
using Nomium.Core.Data.Entities;
using RRMonitoring.Identity.Domain.Enums;

namespace RRMonitoring.Identity.Domain.Entities;

public class UserEvent : EntityBase
{
	public Guid UserId { get; set; }
	public User User { get; set; }

	public UserEventKinds EventKind { get; set; }

	public DateTime EventDate { get; set; }
	public DateTime? BlockEndDate { get; set; }
}
