using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Services.TwoFactor.Notification;

public class TwoFactorNotification : NotificationBase
{
	public string Code { get; set; }

	public string Username { get; set; }

	public TwoFactorNotification(Channels channel)
		: base(channel)
	{
	}
}
