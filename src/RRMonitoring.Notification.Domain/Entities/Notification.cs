using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Notification.Domain.Entities;

public class Notification : EntityBase
{
	public string Identifier { get; set; }

	public string Description { get; set; }

	public int GroupId { get; set; }
	public NotificationGroup Group { get; set; }

	public ICollection<Template> Templates { get; set; }
}
