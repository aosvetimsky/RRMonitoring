using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Services.Registration.Notification;

public class UserRegistrationNotification : EmailNotification
{
	public string Link { get; set; }

	public string Username { get; set; }
}
