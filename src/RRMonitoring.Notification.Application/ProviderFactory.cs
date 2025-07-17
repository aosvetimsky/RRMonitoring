using System;
using Microsoft.Extensions.DependencyInjection;
using RRMonitoring.Notification.Application.Providers;
using RRMonitoring.Notification.Application.Providers.Email;
using RRMonitoring.Notification.Application.Providers.Push;
using RRMonitoring.Notification.Application.Providers.Sms;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application;

public class ProviderFactory(IServiceProvider services)
{
	public INotificationProvider GetProvider(Channels channel)
	{
		return channel switch
		{
			Channels.Email => services.GetRequiredService<IEmailProvider>(),
			Channels.Push => services.GetRequiredService<IPushProvider>(),
			Channels.Sms => services.GetRequiredService<ISmsProvider>(),
			_ => throw new NotImplementedException()
		};
	}
}
