using FluentValidation;
using FluentValidation.AspNetCore;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomium.Core.Application.Extensions;
using Nomium.Core.MediatR.Extensions;
using RRMonitoring.Notification.Application.Configuration;
using RRMonitoring.Notification.Application.Configuration.Providers;
using RRMonitoring.Notification.Application.Features.Notification.Send;
using RRMonitoring.Notification.Application.Interceptors;
using RRMonitoring.Notification.Application.Providers.Email;
using RRMonitoring.Notification.Application.Providers.Email.MailoPost;
using RRMonitoring.Notification.Application.Providers.Email.Smtp;
using RRMonitoring.Notification.Application.Providers.Push;
using RRMonitoring.Notification.Application.Providers.Push.Firebase;
using RRMonitoring.Notification.Application.Providers.Push.SignalR;
using RRMonitoring.Notification.Application.Providers.Sms;
using RRMonitoring.Notification.Application.Providers.Sms.SmsAero;
using RRMonitoring.Notification.Application.Services.Notification;

namespace RRMonitoring.Notification.Application.Extensions;

public static class ServiceCollectionExtensions
{
	private const string ProvidersSectionName = "NotificationProviders";

	public static IServiceCollection AddApplicationsReferences(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatRCore(nameof(RRMonitoring));

		services.AddTransient<IValidator<SendNotificationRequest>, SendNotificationRequestValidator>();
		services.AddTransient<IValidatorInterceptor, RequestsValidationInterceptor>();

		return services;
	}

	public static IServiceCollection AddNotificationProviders(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.RegisterNotificationProviderConfigurations(configuration);
		services.RegisterNotificationProviders(configuration);

		services.AddScoped<INotificationService, NotificationService>();

		services.AddScoped<ProviderFactory>();

		return services;
	}

	private static IServiceCollection RegisterNotificationProviderConfigurations(this IServiceCollection services, IConfiguration configuration)
	{
		var notificationProviders = configuration.GetRequiredSectionValue<NotificationProvidersConfiguration>(ProvidersSectionName);

		if (!string.IsNullOrEmpty(notificationProviders.EmailProvider))
		{
			var section = configuration.GetRequiredSection(notificationProviders.EmailProvider);

			services.AddConfiguration(section, notificationProviders.EmailProvider);
		}

		if (!string.IsNullOrEmpty(notificationProviders.PushProvider))
		{
			var section = configuration.GetRequiredSection(notificationProviders.PushProvider);

			services.AddConfiguration(section, notificationProviders.PushProvider);
		}

		if (!string.IsNullOrEmpty(notificationProviders.SmsProvider))
		{
			var section = configuration.GetRequiredSection(notificationProviders.SmsProvider);

			services.AddConfiguration(section, notificationProviders.SmsProvider);
		}

		return services;
	}

	private static IServiceCollection RegisterNotificationProviders(this IServiceCollection services, IConfiguration configuration)
	{
		var notificationProviders = configuration.GetRequiredSectionValue<NotificationProvidersConfiguration>(ProvidersSectionName);

		if (!string.IsNullOrEmpty(notificationProviders.EmailProvider))
		{
			services.AddProvider(notificationProviders.EmailProvider);
		}

		if (!string.IsNullOrEmpty(notificationProviders.PushProvider))
		{
			services.AddProvider(notificationProviders.PushProvider);
		}

		if (!string.IsNullOrEmpty(notificationProviders.SmsProvider))
		{
			services.AddProvider(notificationProviders.SmsProvider);
		}

		return services;
	}

	private static IServiceCollection AddProvider(this IServiceCollection services, string sectionName)
	{
		switch (sectionName)
		{
			case nameof(SmtpEmailProviderConfiguration):
				services.AddScoped<IEmailProvider, SmtpEmailProvider>();
				services.AddScoped<ISmtpClient, SmtpClient>();
				break;
			case nameof(MailoPostProviderConfiguration):
				services.AddHttpClient<IEmailProvider, MailoPostEmailProvider>();
				break;
			case nameof(FirebasePushProviderConfiguration):
				services.AddScoped<IPushProvider, FirebasePushProvider>();
				services.AddSingleton<IFirebaseMessagingWrapper, FirebaseMessagingWrapper>();
				break;
			case nameof(SignalRPushProviderConfiguration):
				services.AddScoped<IPushProvider, SignalRPushProvider>();
				services.AddSingleton<ISignalRUsersService, SignalRUsersService>();
				break;
			case nameof(SmsAeroProviderConfiguration):
				services.AddHttpClient<ISmsProvider, SmsAeroProvider>();
				break;
		}

		return services;
	}

	private static IServiceCollection AddConfiguration(this IServiceCollection services, IConfigurationSection section, string sectionName)
	{
		switch (sectionName)
		{
			case nameof(SmtpEmailProviderConfiguration):
				services.Configure<SmtpEmailProviderConfiguration>(section);
				break;
			case nameof(MailoPostProviderConfiguration):
				services.Configure<MailoPostProviderConfiguration>(section);
				break;
			case nameof(FirebasePushProviderConfiguration):
				services.Configure<FirebasePushProviderConfiguration>(section);
				break;
			case nameof(SignalRPushProviderConfiguration):
				services.Configure<SignalRPushProviderConfiguration>(section);
				break;
			case nameof(SmsAeroProviderConfiguration):
				services.Configure<SmsAeroProviderConfiguration>(section);
				break;
		}

		return services;
	}
}
