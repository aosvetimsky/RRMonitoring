using System.Threading;
using System.Threading.Tasks;

namespace RRMonitoring.Equipment.Agent.Application.Pipelines;

public interface IScanPipeline
{
	Task<ScanResult> RunAsync(CancellationToken cancellationToken);
}
