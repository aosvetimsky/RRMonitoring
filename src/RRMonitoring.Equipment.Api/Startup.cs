using Asp.Versioning.ApiExplorer;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomium.Core.Application.Extensions;
using Nomium.Core.AutoMapper.Extensions;
using Nomium.Core.FileStorage.Extensions;
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
using RRMonitoring.Equipment.Api.Extensions;
using RRMonitoring.Equipment.Application.Configuration;
using RRMonitoring.Equipment.Application.Extensions;
using RRMonitoring.Equipment.Infrastructure.Extensions;
using ClassFromApplicationAssembly = RRMonitoring.Equipment.Application.Extensions.ServiceCollectionExtensions;

namespace RRMonitoring.Equipment.Api;

public class Startup(IConfiguration configuration, IWebHostEnvironment environment)
{
	private const string AssemblyPrefix = "RRMonitoring.Equipment";
	private const string CorsName = "RRMonitoring.Equipment";
	private const string S3SectionName = "S3FileProvider";

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

		services.Configure<EquipmentS3FileProviderConfiguration>(options =>
			configuration.GetSection(S3SectionName).Bind(options));
		var s3Configuration = configuration.GetRequiredSectionValue<EquipmentS3FileProviderConfiguration>(S3SectionName);
		services.AddS3FileProvider(s3Configuration);

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

		var s3Configuration = configuration.GetRequiredSectionValue<EquipmentS3FileProviderConfiguration>(S3SectionName);
		app.UseS3Buckets(s3Configuration);

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseInternalCurrentUserMiddleware<CurrentUserBase>();
		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
		});
	}
}
