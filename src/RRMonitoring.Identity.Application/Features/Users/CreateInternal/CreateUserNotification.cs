using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.Users.CreateInternal;

public class CreateUserNotification : NotificationBase
{
	public string UserFullName { get; set; }

	public string Url { get; set; }

	public CreateUserNotification(Channels channel) : base(channel)
	{
	}
}
