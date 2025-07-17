using System.Threading;
using System.Threading.Tasks;
using Hangfire.Server;
using MassTransit;
using Microsoft.Extensions.Logging;
using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;
using RRMonitoring.Equipment.Agent.Application.Pipelines;
using RRMonitoring.Equipment.Agent.Infrastructure.Cache;
using RRMonitoring.Equipment.Scanner.BusEvents.Scanner.Events;

namespace RRMonitoring.Equipment.Agent.Application.Jobs;

public class ScanJob(
	IScanPipelineFactory strategies,
	IScanStateStore store,
	IPublishEndpoint bus,
	ILogger<ScanJob> logger)
{
	public async Task ExecuteAsync(
		StartScanCommand command,
		PerformContext context,
		CancellationToken cancellationToken)
	{
		await store.SetRunningAsync(command.ScanId, context.BackgroundJob.Id);
		await RunPipelineAsync(command, cancellationToken);
	}

	public async Task ResumeAsync(
		StartScanCommand command,
		PerformContext context,
		CancellationToken cancellationToken)
	{
		await store.SetRunningAsync(command.ScanId, context.BackgroundJob.Id);
		await RunPipelineAsync(command, cancellationToken);
	}

	private async Task RunPipelineAsync(StartScanCommand command, CancellationToken cancellationToken)
	{
		var pipeline = strategies.Build(command);
		var result = await pipeline.RunAsync(cancellationToken);

		await bus.Publish(new ScanCompletedEvent(command.ScanId, result.FoundDevices), cancellationToken);
		await store.SetFinishedAsync(command.ScanId);

		logger.LogInformation("Scan {ScanId} finished: {Count} devices", command.ScanId, result.FoundDevices.Count);
	}
}
