using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using RRMonitoring.Notification.ApiClients.Enums;

namespace RRMonitoring.Notification.ApiClients.ApiClients.Notification.Models;

internal class SendNotificationRequest
{
	public string Identifier { get; set; }

	public Channels Channel { get; set; }

	public Guid? RecipientId { get; set; }

	public string Recipient { get; set; }

	public string UserData { get; set; }

	public IList<IFormFile> Files { get; set; }
}
