using System;
using System.Threading.Tasks;
using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;

namespace RRMonitoring.Equipment.Agent.Infrastructure.Cache;

public interface IScanStateStore
{
	Task SetRunningAsync(Guid scanId, string hangfireJobId);

	Task SetFinishedAsync(Guid scanId);

	Task<ScanStateResult> TryPauseAsync(Guid scanId);

	Task<(ScanStateResult result, StartScanCommand OriginalCmd)> TryResumeAsync(Guid scanId);

	Task<ScanStateResult> TryCancelAsync(Guid scanId);
}
