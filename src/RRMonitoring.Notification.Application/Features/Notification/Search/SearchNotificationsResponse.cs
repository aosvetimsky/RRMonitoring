using System;

namespace RRMonitoring.Notification.Application.Features.Notification.Search;

public class SearchNotificationsResponse
{
	public Guid Id { get; set; }
	public string Identifier { get; set; }
	public string Description { get; set; }
}
