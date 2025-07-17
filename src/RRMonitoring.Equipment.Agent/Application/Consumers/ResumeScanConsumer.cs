using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.Logging;
using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;
using RRMonitoring.Equipment.Agent.Application.Jobs;
using RRMonitoring.Equipment.Agent.Infrastructure.Cache;
using RRMonitoring.Equipment.Scanner.BusEvents.Scanner.Events;

namespace RRMonitoring.Equipment.Agent.Application.Consumers;

public class ResumeScanConsumer(
	IScanStateStore store,
	IBackgroundJobClient jobs,
	ILogger<ResumeScanConsumer> log)
	: IConsumer<ResumeScanCommand>
{
	public async Task Consume(ConsumeContext<ResumeScanCommand> context)
	{
		var scanId = context.Message.ScanId;

		var (scanStateResult, command) = await store.TryResumeAsync(scanId);

		if (!scanStateResult.Success || command is null)
		{
			log.LogWarning("Scan {ScanId} resume ignored (not paused)", scanId);
			return;
		}

		jobs.Enqueue<ScanJob>(j => j.ResumeAsync(command, null!, CancellationToken.None));

		await context.Publish(new ScanResumedEvent(scanId, DateTimeOffset.UtcNow));

		log.LogInformation("Scan {ScanId} resumed", scanId);
	}
}
