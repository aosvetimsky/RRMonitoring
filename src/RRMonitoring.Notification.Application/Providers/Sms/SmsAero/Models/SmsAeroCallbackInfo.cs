using RRMonitoring.Notification.Application.Providers.Models;

namespace RRMonitoring.Notification.Application.Providers.Sms.SmsAero.Models;

public sealed class SmsAeroCallbackInfo : CallbackInfo
{
	public int Id { get; set; }

	public int Status { get; set; }

	public string ExtendStatus { get; set; }
}
