using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.Logging;
using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;
using RRMonitoring.Equipment.Agent.Application.Jobs;

namespace RRMonitoring.Equipment.Agent.Application.Consumers;

public class StartScanConsumer(
	IBackgroundJobClient jobs,
	ILogger<StartScanConsumer> log
)
	: IConsumer<StartScanCommand>
{
	public Task Consume(ConsumeContext<StartScanCommand> context)
	{
		var command = context.Message;

		log.LogInformation("Start scan {ScanId} ({Name})", command.ScanId, command.Name);

		jobs.Enqueue<ScanJob>(j => j.ExecuteAsync(command, null!, CancellationToken.None));

		return Task.CompletedTask;
	}
}
