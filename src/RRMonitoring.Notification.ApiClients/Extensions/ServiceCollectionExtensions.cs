using Microsoft.Extensions.DependencyInjection;
using RRMonitoring.Notification.ApiClients.ApiClients.Notification.Http;
using RRMonitoring.Notification.ApiClients.ApiClients.Notification.Rabbit;
using RRMonitoring.Notification.ApiClients.Configuration;
using RRMonitoring.Notification.ApiClients.Service.Notification.Http;
using RRMonitoring.Notification.ApiClients.Service.Notification.Rabbit;
using RRMonitoring.Notification.ApiClients.Service.NotificationHistory;

namespace RRMonitoring.Notification.ApiClients.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection RegisterHttpNotificationManager(
		this IServiceCollection services,
		NotificationHttpConfiguration configuration)
	{
		services.AddScoped<IHttpNotificationManager, HttpNotificationManager>();
		services.AddHttpClient<INotificationApiClient, NotificationApiClient>();

		services.Configure<NotificationHttpConfiguration>(config =>
		{
			config.Url = configuration.Url;
			config.ApiKey = configuration.ApiKey;
		});

		return services;
	}

	public static IServiceCollection RegisterRabbitNotificationManager(this IServiceCollection services)
	{
		services.AddScoped<IRabbitNotificationManager, RabbitNotificationManager>();
		services.AddScoped<INotificationRabbitProducer, NotificationRabbitProducer>();

		return services;
	}

	public static IServiceCollection RegisterNotificationApiServices(this IServiceCollection services)
	{
		services.AddScoped<INotificationHistoryService, NotificationHistoryService>();

		return services;
	}
}
