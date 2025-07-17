using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RRMonitoring.Equipment.Scanner.BusEvents.Models;
using StackExchange.Redis;

namespace RRMonitoring.Equipment.Agent.Infrastructure.Cache;

public static class RedisKeys
{
	private const string Prefix = "scanner";

	public static string ScanMeta(Guid scanId) => $"{Prefix}:scan:{scanId}:meta";

	public static string ScanProgress(Guid scanId) => $"{Prefix}:scan:{scanId}:progress";

	public static string AliveSet(string cidr) => $"{Prefix}:alive:{cidr}";

	public static string Device(string ip) => $"{Prefix}:device:{ip}";

	public static string ScanLock(Guid scanId) => $"{Prefix}:scan:{scanId}:lock";
}

public static class RedisDatabaseExtensions
{
	public static Task HashSetScanMetaAsync(this IDatabase db, Guid id, HashEntry[] entries) =>
		db.HashSetAsync(RedisKeys.ScanMeta(id), entries);

	public static Task<long> IncrementProgressAsync(this IDatabase db, Guid id, string field, long delta = 1) =>
		db.HashIncrementAsync(RedisKeys.ScanProgress(id), field, delta);

	public static Task<bool> AddAliveAsync(this IDatabase db, string cidr, string ip) =>
		db.SetAddAsync(RedisKeys.AliveSet(cidr), ip);

	public static async Task<DeviceInfo> TryGetDeviceAsync(this IDatabase db, string ip)
	{
		var raw = await db.HashGetAsync(RedisKeys.Device(ip), "json");

		return raw.ToDeviceInfo();
	}
}

public static class RedisValueExtensions
{
	public static DeviceInfo ToDeviceInfo(this RedisValue value)
	{
		return value.IsNullOrEmpty
			? null
			: JsonSerializer.Deserialize<DeviceInfo>(value);
	}

	public static bool StatusIs(this HashEntry[] meta, ScanStatus status, out string jobId)
	{
		var s = GetJobStatus(meta, out jobId);

		return Enum.TryParse<ScanStatus>(s, out var st) && st == status;
	}

	public static bool StatusIsRunningLike(this HashEntry[] meta, out string jobId)
	{
		var s = GetJobStatus(meta, out jobId);

		return s is nameof(ScanStatus.Running) or nameof(ScanStatus.Paused);
	}

	private static string GetValue(this HashEntry[] meta, string name)
	{
		return meta.FirstOrDefault(h => h.Name == name).Value!;
	}

	private static string GetJobStatus(HashEntry[] meta, out string jobId)
	{
		jobId = meta.GetValue("job");
		var s = meta.GetValue("status");

		return s;
	}
}
