using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Nomium.Core.Application.Extensions;
using RRMonitoring.Notification.Application.Configuration.Providers;
using RRMonitoring.Notification.Application.Providers.Push.SignalR;

namespace RRMonitoring.Notification.Api.Extensions;

public static class EndpointRouteBuilderExtensions
{
	public static IEndpointRouteBuilder MapSignalRHubs(
		this IEndpointRouteBuilder endpointRouteBuilder,
		IConfiguration configuration)
	{
		var signalRSettings = configuration
			.GetRequiredSectionValue<SignalRPushProviderConfiguration>(nameof(SignalRPushProviderConfiguration));

		endpointRouteBuilder.MapHub<SignalRPushHub>(signalRSettings.Endpoint);

		return endpointRouteBuilder;
	}
}
