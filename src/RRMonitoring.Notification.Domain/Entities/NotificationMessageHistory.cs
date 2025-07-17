using System;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Notification.Domain.Entities;

public class NotificationMessageHistory : AuditableEntity
{
	public Guid NotificationMessageId { get; set; }
	public NotificationMessage NotificationMessage { get; set; }

	public byte StatusId { get; set; }
	public Status Status { get; set; }

	public string ErrorText { get; set; }

	public string ExternalSystemStatus { get; set; }
}
