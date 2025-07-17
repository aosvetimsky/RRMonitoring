using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using RRMonitoring.Equipment.Scanner.BusEvents.Models;

namespace RRMonitoring.Equipment.Agent.Application.Detectors;

public class DefaultHandshakeDetector : IHandshakeDetector
{
	public async Task<DeviceInfo> TryDetectAsync(string ip, CancellationToken cancellationToken)
	{
		try
		{
			using var client = new TcpClient();
			await client.ConnectAsync(ip, 4028, cancellationToken);
			await using var stream = client.GetStream();
			var request = "{\"command\":\"version\"}\n";

			await stream.WriteAsync(System.Text.Encoding.ASCII.GetBytes(request), cancellationToken);
			var buffer = new byte[2048];
			var read = await stream.ReadAsync(buffer, cancellationToken);

			var response = System.Text.Encoding.ASCII.GetString(buffer, 0, read);

			if (response.Contains("CGMiner"))
			{
				return new DeviceInfo(ip) { Model = "CGMiner" };
			}
		}
		catch
		{
			/* ignore */
		}

		return null;
	}
}
