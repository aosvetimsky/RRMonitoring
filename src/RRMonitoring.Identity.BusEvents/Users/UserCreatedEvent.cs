using System;

namespace RRMonitoring.Identity.BusEvents.Users;

public class UserCreatedEvent
{
	public Guid Id { get; set; }
}
