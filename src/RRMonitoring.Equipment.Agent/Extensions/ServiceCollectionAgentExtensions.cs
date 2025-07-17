using System.Linq;
using System.Reflection;
using Hangfire;
using Hangfire.Redis.StackExchange;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomium.Core.Application.Extensions;
using Nomium.Core.Cache.Configuration;
using Nomium.Core.MassTransit.Configuration;
using Nomium.Core.MassTransit.Extensions;
using RRMonitoring.Equipment.Agent.Application.Detectors;
using RRMonitoring.Equipment.Agent.Application.Pipelines;
using RRMonitoring.Equipment.Agent.Application.Pipelines.Steps;
using RRMonitoring.Equipment.Agent.Infrastructure.Cache;
using StackExchange.Redis;

namespace RRMonitoring.Equipment.Agent.Extensions;

public static class ServiceCollectionAgentExtensions
{
	public static IServiceCollection AddAgentReferences(
		this IServiceCollection services,
		IConfiguration configuration,
		Assembly consumersAssembly)
	{
		var redisConfiguration = configuration.GetRequiredSectionValue<RedisConfiguration>("Redis");
		services.AddSingleton<IConnectionMultiplexer>(_ =>
			ConnectionMultiplexer.Connect($"{redisConfiguration.Host}:{redisConfiguration.Port}"));

		var rabbitMqConfiguration = configuration.GetRequiredSectionValue<RabbitMqConfiguration>("RabbitMq");
		services.AddMassTransitRabbitMq(rabbitMqConfiguration, opts => opts.AddConsumers(consumersAssembly));

		services.AddHangfire((serviceProvider, globalConfiguration) =>
		{
			var multiplexer = serviceProvider.GetRequiredService<IConnectionMultiplexer>();
			globalConfiguration.UseRedisStorage(multiplexer);
		});
		services.AddHangfireServer();

		services.AddSingleton<IDeviceCache, RedisDeviceCache>();

		// services.AddHostedService<RecurringJobScheduler>();

		return services;
	}

	public static IServiceCollection AddScanSteps(this IServiceCollection services, Assembly assembly)
	{
		services.AddScoped<IHandshakeDetector, DefaultHandshakeDetector>();
		services.AddScoped<IScanPipelineFactory, DefaultScanPipelineFactory>();

		services.AddSingleton<IScanStateStore>(sp =>
		{
			var db = sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase();

			return new ScanStateStoreRedis(db);
		});

		var stepType = typeof(IScanStep);

		foreach (var type in assembly.GetTypes().Where(t => !t.IsAbstract && stepType.IsAssignableFrom(t)))
		{
			services.AddScoped(stepType, type);
		}

		return services;
	}
}
