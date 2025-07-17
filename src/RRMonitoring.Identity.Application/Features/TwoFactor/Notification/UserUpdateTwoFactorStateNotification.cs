using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.TwoFactor.Notification;

public class UserUpdateTwoFactorStateNotification : NotificationBase
{
	public string ChangeDate { get; set; }

	public string Action { get; set; }

	public UserUpdateTwoFactorStateNotification(Channels channel)
		: base(channel)
	{
	}
}
