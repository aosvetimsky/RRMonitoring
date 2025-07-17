using System.Collections.Generic;
using Nomium.Core.Models;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Domain.Models;

public class SearchNotificationMessageHistoryCriteria : PagedRequest
{
	public string Keyword { get; set; }
	public IList<Channels> Channels { get; set; }
	public DateTimePeriod? DatePeriod { get; set; }
	public IList<NotificationStatuses> Statuses { get; set; }
}
