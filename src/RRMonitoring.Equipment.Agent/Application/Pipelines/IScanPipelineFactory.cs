using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;

namespace RRMonitoring.Equipment.Agent.Application.Pipelines;

public interface IScanPipelineFactory
{
	IScanPipeline Build(StartScanCommand command);
}
