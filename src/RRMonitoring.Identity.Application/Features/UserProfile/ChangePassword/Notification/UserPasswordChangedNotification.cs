using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.UserProfile.ChangePassword.Notification;

public class UserPasswordChangedNotification : NotificationBase
{
	public string ChangeDate { get; init; }

	public string Username { get; init; }

	public UserPasswordChangedNotification(Channels channel)
		: base(channel)
	{
	}
}
