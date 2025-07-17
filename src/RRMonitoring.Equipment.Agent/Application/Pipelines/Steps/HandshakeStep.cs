using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;
using RRMonitoring.Equipment.Agent.Application.Detectors;
using RRMonitoring.Equipment.Scanner.BusEvents.Models;

namespace RRMonitoring.Equipment.Agent.Application.Pipelines.Steps;

public class HandshakeStep(IHandshakeDetector detector) : IScanStep
{
	public async Task ExecuteAsync(
		StartScanCommand command,
		List<DeviceInfo> buffer,
		CancellationToken cancellationToken)
	{
		var tasks = buffer.Select(dev => Enrich(dev, cancellationToken)).ToArray();
		await Task.WhenAll(tasks);
	}

	private async Task Enrich(DeviceInfo dev, CancellationToken cancellationToken)
	{
		var info = await detector.TryDetectAsync(dev.Ip, cancellationToken);

		if (info == null)
		{
			return;
		}

		dev.Vendor = info.Vendor;
		dev.Model = info.Model;
		dev.Firmware = info.Firmware;
		dev.ApiType = info.ApiType;
	}
}
