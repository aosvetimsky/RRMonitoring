using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;
using RRMonitoring.Equipment.Scanner.BusEvents.Models;

namespace RRMonitoring.Equipment.Agent.Application.Pipelines.Steps;

public class TcpSynStep : IScanStep
{
	private static readonly int[] Ports = [4028, 4029, 80, 443];

	public async Task ExecuteAsync(
		StartScanCommand command,
		List<DeviceInfo> buffer,
		CancellationToken cancellationToken)
	{
		var tasks = buffer
			.SelectMany(device => Ports.Select(port => Check(device, port, cancellationToken)))
			.ToArray();

		await Task.WhenAll(tasks);
	}

	private static async Task Check(DeviceInfo device, int port, CancellationToken cancellationToken)
	{
		try
		{
			using var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
			{
				ReceiveTimeout = 500,
				SendTimeout = 500
			};

			var connectTask = sock.ConnectAsync(device.Ip, port, cancellationToken).AsTask();

			if (await Task.WhenAny(connectTask, Task.Delay(600, cancellationToken)) == connectTask && sock.Connected)
			{
				device.OpenPorts.Add(port);
			}
		}
		catch
		{
			/* ignore */
		}
	}
}
