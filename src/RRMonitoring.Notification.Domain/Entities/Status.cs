using Nomium.Core.Data.Entities;

namespace RRMonitoring.Notification.Domain.Entities;

public class Status : EntityBase<byte>
{
	public string Name { get; set; }
}
