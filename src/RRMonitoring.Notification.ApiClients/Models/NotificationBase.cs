using System;
using RRMonitoring.Notification.ApiClients.Enums;

namespace RRMonitoring.Notification.ApiClients.Models;

public abstract class NotificationBase(Channels channel)
{
	public Guid? RecipientId { get; set; }

	public string Recipient { get; set; }

	public Channels Channel { get; private init; } = channel;
}
