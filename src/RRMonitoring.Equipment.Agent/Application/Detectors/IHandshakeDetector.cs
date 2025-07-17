using System.Threading;
using System.Threading.Tasks;
using RRMonitoring.Equipment.Scanner.BusEvents.Models;

namespace RRMonitoring.Equipment.Agent.Application.Detectors;

public interface IHandshakeDetector
{
	Task<DeviceInfo> TryDetectAsync(string ip, CancellationToken cancellationToken);
}
