using System;
using System.Threading.Tasks;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.Logging;
using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;
using RRMonitoring.Equipment.Agent.Infrastructure.Cache;
using RRMonitoring.Equipment.Scanner.BusEvents.Scanner.Events;

namespace RRMonitoring.Equipment.Agent.Application.Consumers;

public class CancelScanConsumer(
	IScanStateStore store,
	IBackgroundJobClient jobs,
	ILogger<CancelScanConsumer> log)
	: IConsumer<CancelScanCommand>
{
	public async Task Consume(ConsumeContext<CancelScanCommand> context)
	{
		var id = context.Message.ScanId;
		var result = await store.TryCancelAsync(id);

		if (result is { Success: true, HangfireJobId: { } jobId })
		{
			jobs.Delete(jobId);
			await context.Publish(new ScanCancelledEvent(id, DateTimeOffset.UtcNow));
			log.LogInformation("Scan {ScanId} cancelled", id);
		}
		else
		{
			log.LogWarning("Scan {ScanId} cancel ignored (unknown or finished)", id);
		}
	}
}
