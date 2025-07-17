namespace RRMonitoring.Notification.Domain.Enums;

public enum NotificationStatuses : byte
{
	Queued = 1,
	Delivered,
	Failed
}
