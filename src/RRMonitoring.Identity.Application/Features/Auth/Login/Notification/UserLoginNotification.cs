using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.Auth.Login.Notification;

public sealed class UserLoginNotification : EmailNotification
{
	public string Username { get; set; }

	public string LoginDate { get; set; }
}
