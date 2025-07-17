using System;
using System.Reflection;
using Asp.Versioning.ApiExplorer;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomium.Core.Application.Extensions;
using Nomium.Core.AutoMapper.Extensions;
using Nomium.Core.Security.Configuration;
using Nomium.Core.Security.Extensions;
using Nomium.Core.Swagger.Configuration;
using Nomium.Core.Swagger.Extensions;
using Nomium.Core.Swagger.Extensions.DependencyInjection;
using Prometheus;
using Prometheus.SystemMetrics;
using RRMonitoring.Bff.Admin.Application.Extensions;
using RRMonitoring.Common.OpenTelemetry.Extensions;
using ClassFromApplicationAssembly = RRMonitoring.Bff.Admin.Application.Extensions.ServiceCollectionExtensions;

namespace RRMonitoring.Bff.Admin.Api;

public class Startup(
	IConfiguration configuration,
	IWebHostEnvironment environment)
{
	private const string AssemblyPrefix = "RRMonitoring.Bff.Admin";
	private const string CorsName = "RRMonitoring.Bff.Admin";

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddWebApiCore();
		services.AddControllers();

		if (environment.IsTestEnvironment())
		{
			services.AddCorsBase(CorsName);

			var swaggerSettings = configuration.GetRequiredSectionValue<SwaggerSettings>("Swagger");
			services.AddSwaggerCore(swaggerSettings);

			services.AddFluentValidationRulesToSwagger();
		}

		services.AddAutoMapper(AssemblyPrefix);

		services.AddFluentValidationAutoValidation()
			.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(ClassFromApplicationAssembly)));

		services.AddApplicationReferences(configuration);

		var identitySettings = configuration.GetRequiredSectionValue<IdentitySettings>("Identity");
		services.AddJwtAuthentication(identitySettings, environment, options =>
		{
			options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
		});

		services.AddAuthorization(options =>
		{
			options.AddPermissionPolicies();
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

		app.UseCurrentUserMiddleware();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
		});
	}
}
