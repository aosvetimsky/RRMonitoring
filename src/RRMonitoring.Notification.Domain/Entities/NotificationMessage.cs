using System;
using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Notification.Domain.Entities;

public class NotificationMessage : AuditableEntity
{
	public string RecipientId { get; set; }
	public string RecipientAddress { get; set; }

	public string ExternalMessageId { get; set; }
	public string NotificationBody { get; set; }

	public byte ChannelId { get; set; }
	public Channel Channel { get; set; }

	public Guid? NotificationId { get; set; }
	public Notification Notification { get; set; }

	public ICollection<NotificationMessageHistory> NotificationMessageHistory { get; set; }

	public bool? PushIsRead { get; set; }
}
