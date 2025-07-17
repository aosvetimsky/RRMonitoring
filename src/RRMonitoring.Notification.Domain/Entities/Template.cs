using System;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Notification.Domain.Entities;

public class Template : EntityBase
{
	public byte ChannelId { get; set; }

	public Channel Channel { get; set; }

	public Guid NotificationId { get; set; }

	public Notification Notification { get; set; }

	public string Subject { get; set; }

	public string Data { get; set; }
}
