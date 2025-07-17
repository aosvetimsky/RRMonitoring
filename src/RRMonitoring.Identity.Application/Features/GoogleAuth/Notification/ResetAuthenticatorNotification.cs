using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.GoogleAuth.Notification;

public class ResetAuthenticatorNotification : NotificationBase
{
	public string Username { get; set; }

	public string Code { get; set; }

	public ResetAuthenticatorNotification(Channels channel)
		: base(channel)
	{
	}
}
