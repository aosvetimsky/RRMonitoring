using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdateEmail.Notification;

public sealed class UserUpdateEmailNotification : NotificationBase
{
	public string Username { get; init; }

	public string NewEmail { get; init; }

	public UserUpdateEmailNotification(Channels channel)
		: base(channel)
	{
	}
}
