using System;

namespace RRMonitoring.Identity.Application.Configuration;

public class CacheConfiguration
{
	public TimeSpan MemoryCacheShortLifetime { get; set; }

	public TimeSpan MemoryCacheMediumLifetime { get; set; }

	public TimeSpan MemoryCacheLongLifetime { get; set; }
}
