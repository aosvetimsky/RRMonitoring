using System.Collections.Generic;
using Nomium.Core.Models;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Domain.Models;

public class SearchNotificationMessageCriteria : PagedRequest
{
	public IList<string> RecipientIds { get; set; }

	public IList<Channels> Channels { get; set; }

	public bool IncludeNotification { get; set; }
}
