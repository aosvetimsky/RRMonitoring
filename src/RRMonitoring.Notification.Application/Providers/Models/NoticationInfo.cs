using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace RRMonitoring.Notification.Application.Providers.Models;

public sealed class NotificationInfo
{
	public Guid? NotificationId { get; set; }

	public string Identifier { get; set; }

	public string RecipientId { get; set; }

	public string Recipient { get; set; }

	public string Subject { get; set; }

	public string Body { get; set; }

	public IList<IFormFile> Attachments { get; set; }
}
