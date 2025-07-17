using System;
using System.Text.Json;
using System.Threading.Tasks;
using RRMonitoring.Equipment.Agent.Application.Contracts.Commands;
using StackExchange.Redis;

namespace RRMonitoring.Equipment.Agent.Infrastructure.Cache;

public class ScanStateStoreRedis(IDatabase db)
	: IScanStateStore
{
	public Task SetRunningAsync(Guid scanId, string hangfireJobId)
	{
		return db.HashSetAsync(RedisKeys.ScanMeta(scanId),
			[new HashEntry("status", nameof(ScanStatus.Running)), new HashEntry("job", hangfireJobId)]);
	}

	public Task SetFinishedAsync(Guid scanId)
	{
		return db.HashSetAsync(RedisKeys.ScanMeta(scanId), "status", nameof(ScanStatus.Finished));
	}

	public async Task<ScanStateResult> TryPauseAsync(Guid scanId)
	{
		var meta = await db.HashGetAllAsync(RedisKeys.ScanMeta(scanId));

		if (!meta.StatusIs(ScanStatus.Running, out var jobId))
		{
			return new ScanStateResult(false, null);
		}

		await db.HashSetAsync(RedisKeys.ScanMeta(scanId), "status", nameof(ScanStatus.Paused));

		return new ScanStateResult(true, jobId);
	}

	public async Task<(ScanStateResult, StartScanCommand)> TryResumeAsync(Guid scanId)
	{
		var meta = await db.HashGetAllAsync(RedisKeys.ScanMeta(scanId));

		if (!meta.StatusIs(ScanStatus.Paused, out var jobId))
		{
			return (new ScanStateResult(false, null), null);
		}

		var rawCmd = await db.StringGetAsync(RedisKeys.ScanMeta(scanId) + ":cmd");
		var cmd = JsonSerializer.Deserialize<StartScanCommand>(rawCmd!);

		await db.HashSetAsync(RedisKeys.ScanMeta(scanId), "status", nameof(ScanStatus.Running));

		return (new ScanStateResult(true, jobId), cmd);
	}

	public async Task<ScanStateResult> TryCancelAsync(Guid scanId)
	{
		var meta = await db.HashGetAllAsync(RedisKeys.ScanMeta(scanId));

		if (!meta.StatusIsRunningLike(out var jobId))
		{
			return new ScanStateResult(false, null);
		}

		await db.HashSetAsync(RedisKeys.ScanMeta(scanId), "status", nameof(ScanStatus.Cancelled));

		return new ScanStateResult(true, jobId);
	}
}
