using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.ForgotPassword.SendLink;

internal class ResetPasswordEmailNotification : EmailNotification
{
	public string Username { get; set; }

	public string Link { get; set; }
}
