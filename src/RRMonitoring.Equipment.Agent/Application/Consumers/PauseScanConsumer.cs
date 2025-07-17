using System;
using System.Threading.Tasks;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.Logging;
using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;
using RRMonitoring.Equipment.Agent.Infrastructure.Cache;
using RRMonitoring.Equipment.Scanner.BusEvents.Scanner.Events;

namespace RRMonitoring.Equipment.Agent.Application.Consumers;

public class PauseScanConsumer(
	IScanStateStore store,
	IBackgroundJobClient jobs,
	ILogger<PauseScanConsumer> logger)
	: IConsumer<PauseScanCommand>
{
	public async Task Consume(ConsumeContext<PauseScanCommand> context)
	{
		var result = await store.TryPauseAsync(context.Message.ScanId);

		if (!result.Success)
		{
			return;
		}

		jobs.Delete(result.HangfireJobId);

		await context.Publish(new ScanPausedEvent(context.Message.ScanId, DateTimeOffset.UtcNow));

		logger.LogInformation("Scan {ScanId} paused", context.Message.ScanId);
	}
}
