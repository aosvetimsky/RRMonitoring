using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.GoogleAuth.Notification;

public class UserUpdateAuthenticatorStateNotification : NotificationBase
{
	public string ChangeDate { get; set; }

	public string Action { get; set; }

	public string Username { get; set; }

	public UserUpdateAuthenticatorStateNotification(Channels channel)
		: base(channel)
	{
	}
}
