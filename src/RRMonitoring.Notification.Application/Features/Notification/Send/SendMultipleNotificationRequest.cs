using System.Collections.Generic;

namespace RRMonitoring.Notification.Application.Features.Notification.Send;

public class SendMultipleNotificationRequest
{
	public IList<SendNotificationRequest> NotificationRequests { get; set; }
}
