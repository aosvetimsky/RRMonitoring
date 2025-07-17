using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;
using RRMonitoring.Equipment.Scanner.BusEvents.Models;

namespace RRMonitoring.Equipment.Agent.Application.Pipelines.Steps;

public interface IScanStep
{
	Task ExecuteAsync(StartScanCommand command, List<DeviceInfo> buffer, CancellationToken cancellationToken);
}
