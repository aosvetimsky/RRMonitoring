using Nomium.Core.Models;

namespace RRMonitoring.Notification.Domain.Models;

public class SearchNotificationsCriteria : PagedRequest
{
	public string Keyword { get; set; }
}
