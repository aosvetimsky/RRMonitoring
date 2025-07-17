using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RRMonitoring.Equipment.Agent.Application.Pipelines.Steps;
using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;
using RRMonitoring.Equipment.Scanner.BusEvents.Models;

namespace RRMonitoring.Equipment.Agent.Application.Pipelines;

public class ScanPipeline(IScanStep[] allSteps, StartScanCommand command)
	: IScanPipeline
{
	public async Task<ScanResult> RunAsync(CancellationToken cancellationToken)
	{
		var found = new List<DeviceInfo>();

		foreach (var step in allSteps)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await step.ExecuteAsync(command, found, cancellationToken);
		}

		return new ScanResult(found);
	}
}
