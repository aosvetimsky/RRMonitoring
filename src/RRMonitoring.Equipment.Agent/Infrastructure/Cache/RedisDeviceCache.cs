using System;
using System.Text.Json;
using System.Threading.Tasks;
using RRMonitoring.Equipment.Scanner.BusEvents.Models;
using StackExchange.Redis;

namespace RRMonitoring.Equipment.Agent.Infrastructure.Cache;

public class RedisDeviceCache(IDatabase db) : IDeviceCache
{
	public Task<DeviceInfo> GetAsync(string ip)
	{
		return db.TryGetDeviceAsync(ip);
	}

	public Task SetAsync(DeviceInfo device)
	{
		var json = JsonSerializer.Serialize(device);

		return db.HashSetAsync(RedisKeys.Device(device.Ip),
		[
			new HashEntry("json", json),
			new HashEntry("updated", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
		]);
	}
}
