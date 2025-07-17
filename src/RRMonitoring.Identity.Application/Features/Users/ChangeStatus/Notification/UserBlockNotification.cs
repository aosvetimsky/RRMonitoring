using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.Users.ChangeStatus.Notification;

public sealed class UserBlockNotification : EmailNotification
{
	public string Username { get; set; }

	public string Reason { get; set; }

	public string BlockDate { get; set; }
}
