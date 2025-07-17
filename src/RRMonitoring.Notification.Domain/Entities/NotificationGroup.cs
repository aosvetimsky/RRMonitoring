using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Notification.Domain.Entities;

public class NotificationGroup : EntityBase<int>
{
	public string Name { get; set; }

	public IList<Notification> Notifications { get; set; }
}
