using System;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Providers.Models;

public sealed class NotificationResult
{
	public Guid? NotificationId { get; set; }

	public string Identifier { get; set; }

	public string RecipientId { get; set; }

	public string Recipient { get; set; }

	public string Subject { get; set; }

	public string Body { get; set; }

	public byte ChannelId { get; set; }

	public string ExternalMessageId { get; set; }

	public bool IsSuccess { get; set; }

	public NotificationStatuses Status { get; set; }

	public string ExternalStatus { get; set; }

	public string Error { get; set; }
}
