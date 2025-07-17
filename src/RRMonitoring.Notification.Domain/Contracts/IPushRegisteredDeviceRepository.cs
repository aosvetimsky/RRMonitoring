using System.Collections.Generic;
using System.Threading.Tasks;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Domain.Contracts;

public interface IPushRegisteredDeviceRepository
{
	Task<PushRegisteredDevice> Get(string recipientId, string deviceId);

	Task<IList<PushRegisteredDevice>> GetByRecipientId(string recipientId);

	Task Add(PushRegisteredDevice device);

	Task Update(PushRegisteredDevice device);

	Task Remove(PushRegisteredDevice device);

	Task RemoveRange(IList<PushRegisteredDevice> devices);
}
