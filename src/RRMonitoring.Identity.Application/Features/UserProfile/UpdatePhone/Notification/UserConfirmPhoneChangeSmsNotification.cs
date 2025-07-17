
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdatePhone.Notification;

public class UserConfirmPhoneChangeSmsNotification : SmsNotification
{
	public string Code { get; init; }
}
