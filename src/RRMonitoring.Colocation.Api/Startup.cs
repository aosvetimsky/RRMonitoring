using Asp.Versioning.ApiExplorer;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomium.Core.Application.Extensions;
using Nomium.Core.AutoMapper.Extensions;
using Nomium.Core.MediatR.Extensions;
using Nomium.Core.OpenTelemetry.Extensions;
using Nomium.Core.Security.Configuration;
using Nomium.Core.Security.Extensions;
using Nomium.Core.Security.Services.CurrentUser.Models;
using Nomium.Core.Swagger.Configuration;
using Nomium.Core.Swagger.Extensions;
using Nomium.Core.Swagger.Extensions.DependencyInjection;
using Prometheus;
using Prometheus.SystemMetrics;
using RRMonitoring.Colocation.Application.Extensions;
using RRMonitoring.Colocation.Infrastructure.Extensions;
using ClassFromApplicationAssembly = RRMonitoring.Colocation.Application.Extensions.ServiceCollectionExtensions;

namespace RRMonitoring.Colocation.Api;

public class Startup(IConfiguration configuration, IWebHostEnvironment environment)
{
	private const string AssemblyPrefix = "RRMonitoring.Colocation";
	private const string CorsName = "RRMonitoring.Colocation";

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddWebApiCore();
		services.AddControllers();
		services.AddHttpContextAccessor();

		if (environment.IsTestEnvironment())
		{
			services.AddCorsBase(CorsName);

			var swaggerSettings = configuration.GetRequiredSectionValue<SwaggerSettings>("Swagger");
			services.AddSwaggerCore(swaggerSettings);

			services.AddFluentValidationRulesToSwagger();
		}

		services.AddAutoMapper(AssemblyPrefix);
		services.AddMediatRCore(AssemblyPrefix);

		services.AddApplicationReferences(configuration);
		services.AddInfrastructureReferences(configuration);

		services
			.AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
			.AddApiKeySupport(options =>
			{
				options.ApiKeyValue = configuration.GetRequiredSectionValue<string>("ApiKey");
			});

		services.AddHealthChecks();

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

		app.UseHealthChecks("/health");
		app.UseMetricServer();

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseInternalCurrentUserMiddleware<CurrentUserBase>();
		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
		});
	}
}
