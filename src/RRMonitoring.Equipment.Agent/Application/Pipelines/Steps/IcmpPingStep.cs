using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;
using RRMonitoring.Equipment.Scanner.BusEvents.Models;

namespace RRMonitoring.Equipment.Agent.Application.Pipelines.Steps;

public class IcmpPingStep : IScanStep
{
	private const int Timeout = 100;

	public async Task ExecuteAsync(
		StartScanCommand command,
		List<DeviceInfo> buffer,
		CancellationToken cancellationToken)
	{
		using var ping = new Ping();
		var bag = new ConcurrentBag<DeviceInfo>();

		var addresses = IpRangeEnumerator.Expand(command.IpMask, command.TotalLimit);

		await Parallel.ForEachAsync(addresses,
			new ParallelOptions
			{
				MaxDegreeOfParallelism = 128,
				CancellationToken = cancellationToken
			},
			async (ip, token) =>
			{
				try
				{
					var reply = await ping.SendPingAsync(ip, Timeout);

					if (reply.Status == IPStatus.Success)
					{
						bag.Add(new DeviceInfo(ip));
					}
				}
				catch
				{
					/* ignore timeouts */
				}
			});

		buffer.AddRange(bag);
	}
}
