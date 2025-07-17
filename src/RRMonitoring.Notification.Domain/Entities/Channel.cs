using Nomium.Core.Data.Entities;

namespace RRMonitoring.Notification.Domain.Entities;

public class Channel : EntityBase<byte>
{
	public string Name { get; set; }
}
