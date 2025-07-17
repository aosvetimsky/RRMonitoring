namespace RRMonitoring.Notification.Application.Providers.Sms.SmsAero.Enums;

internal enum SmsAeroMessageStatus
{
	Queue = 0,
	Delivered,
	NoDelivered,
	Transferred,
	OnModeration,
	Rejected,
	Waiting
}
