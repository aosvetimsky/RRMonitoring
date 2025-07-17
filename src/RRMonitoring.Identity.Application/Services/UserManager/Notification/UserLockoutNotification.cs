using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Services.UserManager.Notification;

public sealed class UserLockoutNotification : EmailNotification
{
	public string Username { get; set; }

	public string EndDateLockout { get; set; }
}
