using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.Users.ChangeStatus.Notification;

public sealed class UserUnblockNotification : EmailNotification
{
	public string Username { get; set; }
}
