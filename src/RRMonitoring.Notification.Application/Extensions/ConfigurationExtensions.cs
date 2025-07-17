using Microsoft.Extensions.Configuration;
using Nomium.Core.Application.Extensions;
using RRMonitoring.Notification.Application.Configuration;

namespace RRMonitoring.Notification.Application.Extensions;

public static class ConfigurationExtensions
{
	public static bool IsPushNotificationProviderEnabled(this IConfiguration configuration, string providerName)
	{
		var notificationProviders =
			configuration.GetRequiredSectionValue<NotificationProvidersConfiguration>("NotificationProviders");

		return notificationProviders.PushProvider == providerName;
	}
}
