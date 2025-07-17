using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;
using RRMonitoring.Equipment.Agent.Application.Pipelines.Steps;

namespace RRMonitoring.Equipment.Agent.Application.Pipelines;

public class DefaultScanPipelineFactory(IScanStep[] steps)
	: IScanPipelineFactory
{
	public IScanPipeline Build(StartScanCommand command)
	{
		return new ScanPipeline(steps, command);
	}
}
