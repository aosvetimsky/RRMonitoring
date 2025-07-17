using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Infrastructure.Database;
using RRMonitoring.Notification.Infrastructure.Database.Repositories;

namespace RRMonitoring.Notification.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureReferences(
		this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<NotificationContext>(opt =>
			opt.UseNpgsql(configuration.GetConnectionString("NotificationContext")));

		services.AddScoped<INotificationRepository, NotificationRepository>();
		services.AddScoped<INotificationMessageRepository, NotificationMessageRepository>();
		services.AddScoped<INotificationMessageHistoryRepository, NotificationMessageHistoryRepository>();
		services.AddScoped<INotificationGroupRepository, NotificationGroupRepository>();
		services.AddScoped<ITemplateRepository, TemplateRepository>();
		services.AddScoped<IPushRegisteredDeviceRepository, PushRegisteredDeviceRepository>();
		services.AddScoped<IRecipientSettingsRepository, RecipientSettingsRepository>();

		return services;
	}

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
				.AddOtlpExporter());

		return services;
	}
}
