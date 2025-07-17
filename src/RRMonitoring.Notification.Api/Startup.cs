using System.Reflection;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomium.Core.Application.Extensions;
using Nomium.Core.AutoMapper.Extensions;
using Nomium.Core.MassTransit.Configuration;
using Nomium.Core.MassTransit.Extensions;
using Nomium.Core.Security.Configuration;
using Nomium.Core.Security.Extensions;
using Nomium.Core.Swagger.Configuration;
using Nomium.Core.Swagger.Extensions;
using Nomium.Core.Swagger.Extensions.DependencyInjection;
using Prometheus;
using Prometheus.SystemMetrics;
using RRMonitoring.Notification.Api.Extensions;
using RRMonitoring.Notification.Application.Configuration;
using RRMonitoring.Notification.Application.Configuration.Providers;
using RRMonitoring.Notification.Application.Extensions;
using RRMonitoring.Notification.Infrastructure.Extensions;

namespace RRMonitoring.Notification.Api;

public class Startup(
	IConfiguration configuration,
	IWebHostEnvironment environment)
{
	private const string AssemblyPrefix = "RRMonitoring.Notification";
	private const string CorsName = nameof(Notification);

	public void ConfigureServices(IServiceCollection services)
	{
		if (environment.IsTestEnvironment())
		{
			services.AddCors(o => o.AddPolicy(CorsName, options =>
			{
				options.SetIsOriginAllowed(_ => true)
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowCredentials();
			}));
		}

		services.AddControllers();
		services.AddFluentValidationAutoValidation();
		services.AddFluentValidationClientsideAdapters();

		services.Configure<DefaultRecipientSettingsConfiguration>(options =>
			configuration.GetSection("DefaultRecipientSettings").Bind(options));

		services.AddInfrastructureReferences(configuration);
		services.AddApplicationsReferences(configuration);

		services.AddOptions();

		services.AddNotificationProviders(configuration);

		var rabbitMqConfig = configuration.GetRequiredSectionValue<RabbitMqConfiguration>("RabbitMq");
		services.AddMassTransitRabbitMq(rabbitMqConfig, options =>
		{
			options.AddConsumers(Assembly.GetAssembly(typeof(Startup)));
		});

		services.AddAutoMapper(nameof(RRMonitoring));

		services.AddWebApiCore();

		services.AddHealthChecks();

		if (environment.IsTestEnvironment())
		{
			var swaggerSettings = configuration.GetRequiredSectionValue<SwaggerSettings>("Swagger");
			services.AddSwaggerCore(swaggerSettings);
		}

		if (configuration.IsPushNotificationProviderEnabled(nameof(SignalRPushProviderConfiguration)))
		{
			services.AddSignalR();
		}

		var identitySettings = configuration.GetRequiredSectionValue<IdentitySettings>("Identity");

		services
			.AddJwtAuthentication(identitySettings, environment, options =>
			{
				if (configuration.IsPushNotificationProviderEnabled(nameof(SignalRPushProviderConfiguration)))
				{
					var signalRSettings =
						configuration.GetRequiredSectionValue<SignalRPushProviderConfiguration>(
							nameof(SignalRPushProviderConfiguration));

					options.Events = new JwtBearerEvents
					{
						OnMessageReceived = context =>
						{
							var accessToken = context.Request.Query["access_token"];

							var path = context.HttpContext.Request.Path;
							if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments(signalRSettings.Endpoint))
							{
								context.Token = accessToken;
							}

							return Task.CompletedTask;
						}
					};
				}
			})
			.AddApiKeySupport(options =>
			{
				options.ApiKeyValue = configuration.GetRequiredSectionValue<string>("ApiKey");
			});

		services.AddSystemMetrics();
		services.AddApplicationMetrics();

		services.AddOpenTelemetryReferences(AssemblyPrefix);
	}

	public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
	{
		app.UsePathBase(configuration["PathBase"]);

		if (environment.IsTestEnvironment())
		{
			app.UseDeveloperExceptionPage();
			app.UseCors(CorsName);

			var swaggerSettings = configuration.GetRequiredSectionValue<SwaggerSettings>("Swagger");
			app.UseSwaggerBase(provider, swaggerSettings);
		}

		app.UseRouting();

		app.UseExceptionMiddleware();
		app.UseAuthentication();
		app.UseAuthorization();

		app.UseHealthChecks("/health");

		app.UseMetricServer();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();

			if (configuration.IsPushNotificationProviderEnabled(nameof(SignalRPushProviderConfiguration)))
			{
				endpoints.MapSignalRHubs(configuration);
			}
		});
	}
}
