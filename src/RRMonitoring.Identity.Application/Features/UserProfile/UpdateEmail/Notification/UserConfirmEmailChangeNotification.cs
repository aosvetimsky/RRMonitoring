using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdateEmail.Notification;

public class UserConfirmEmailChangeNotification : EmailNotification
{
	public string Code { get; set; }

	public string Username { get; set; }
}
