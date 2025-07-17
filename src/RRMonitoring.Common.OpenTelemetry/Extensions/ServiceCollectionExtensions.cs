using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace RRMonitoring.Common.OpenTelemetry.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddOpenTelemetryReferences(
		this IServiceCollection services,
		string serviceName)
	{
		services.AddOpenTelemetry()
			.ConfigureResource(resource => resource
				.AddService(serviceName: serviceName))
			.WithTracing(tracing => tracing
				.AddAspNetCoreInstrumentation()
				.AddHttpClientInstrumentation()
				.AddEntityFrameworkCoreInstrumentation()
				.AddMassTransitInstrumentation()
				.AddSqlClientInstrumentation()
				.AddRedisInstrumentation()
				.AddNpgsql()
				.AddOtlpExporter());

		return services;
	}
}
