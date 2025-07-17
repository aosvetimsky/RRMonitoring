using System.ComponentModel.DataAnnotations;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Features.RecipientSettings.GetByCurrentUser;

public class UserNotificationSettingResponse
{
	[Required]
	public Channels Channel { get; set; }

	[Required]
	public string NotificationIdentifier { get; set; }

	public string NotificationDescription { get; set; }

	[Required]
	public bool IsEnabled { get; set; }
}
