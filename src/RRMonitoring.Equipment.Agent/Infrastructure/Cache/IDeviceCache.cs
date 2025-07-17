using System.Threading.Tasks;
using RRMonitoring.Equipment.Scanner.BusEvents.Models;

namespace RRMonitoring.Equipment.Agent.Infrastructure.Cache;

public interface IDeviceCache
{
	Task<DeviceInfo> GetAsync(string ip);

	Task SetAsync(DeviceInfo device);
}
