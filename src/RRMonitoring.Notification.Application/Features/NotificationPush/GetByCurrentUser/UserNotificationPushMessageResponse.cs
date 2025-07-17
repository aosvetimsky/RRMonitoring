using System;

namespace RRMonitoring.Notification.Application.Features.NotificationPush.GetByCurrentUser;

public class UserNotificationPushMessageResponse
{
	public string Description { get; set; }
	public DateTime CreatedDate { get; set; }
	public string NotificationBody { get; set; }
}
