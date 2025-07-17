using System;
using System.Collections.Generic;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Features.Notification.GetById;

public sealed class NotificationInfoResponse
{
	public Guid Id { get; set; }
	public string Identifier { get; set; }
	public string Description { get; set; }
	public int GroupId { get; set; }
	public IList<NotificationTemplateResponseItem> Templates { get; set; }
}

public sealed class NotificationTemplateResponseItem
{
	public Guid Id { get; set; }
	public Channels ChannelId { get; set; }
	public string Subject { get; set; }
	public string Data { get; set; }
}
