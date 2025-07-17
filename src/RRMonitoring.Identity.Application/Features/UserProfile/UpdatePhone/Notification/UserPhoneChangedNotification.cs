using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdatePhone.Notification;

public class UserPhoneChangedNotification : NotificationBase
{
	public string Username { get; set; }

	public string ChangeDate { get; init; }

	public UserPhoneChangedNotification(Channels channel)
		: base(channel)
	{
	}
}
