using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.ForgotPassword.Notification;

internal class ResetPasswordNotification : NotificationBase
{
	public string Code { get; set; }

	public string Username { get; set; }

	public ResetPasswordNotification(Channels channel) : base(channel)
	{
	}
}
