using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Hosting;
using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;
using RRMonitoring.Equipment.Agent.Application.Jobs;

namespace RRMonitoring.Equipment.Agent.Application.Scheduling;

public class RecurringJobScheduler : IHostedService
{
	// private readonly IRecurringJobManager _manager;

	public Task StartAsync(CancellationToken cancellationToken)
	{
		//_manager.AddOrUpdate<ScanJob>("refresh-alive", j => j.ExecuteAsync(new StartScanCommand(), null!, cancellationToken), "*/5 * * * *");

		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
