using System;

namespace RRMonitoring.Notification.Application.Features.NotificationsHistory.Search;

public class SearchNotificationHistoryResponse
{
	public string RecipientId { get; set; }
	public string RecipientAddress { get; set; }
	public string ExternalMessageId { get; set; }
	public byte StatusId { get; set; }
	public int GroupId { get; set; }
	public byte ChannelId { get; set; }
	public Guid NotificationId { get; set; }
	public DateTime CreatedDate { get; set; }
	public string NotificationBody { get; set; }
	public string ErrorText { get; set; }
	public string ExternalSystemStatus { get; set; }
}
