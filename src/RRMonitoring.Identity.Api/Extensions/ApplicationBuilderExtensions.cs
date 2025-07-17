using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Nomium.Core.Application.Extensions;
using Nomium.Core.FileStorage.Providers.S3;
using Nomium.Core.FileStorage.Providers.S3.Configuration;
using Nomium.Core.Swagger.Configuration;

namespace RRMonitoring.Identity.Api.Extensions;

public static class ApplicationBuilderExtensions
{
	public static void UseIdentitySwagger(this IApplicationBuilder app, IConfiguration configuration)
	{
		var swaggerSettings = configuration.GetRequiredSectionValue<SwaggerSettings>("Swagger");

		app.UseSwagger(options => options.PreSerializeFilters.Add((doc, request) =>
		{
			doc.Servers = new List<OpenApiServer>
			{
				new() { Url = $"{request.Scheme}://{request.Host.Value}{swaggerSettings.ServerPrefix}" }
			};
		}));

		app.UseSwaggerUI(options =>
		{
			options.SwaggerEndpoint("v1/swagger.json", "Identity");

			options.OAuthClientId(swaggerSettings.Authentication?.ClientId);
			options.OAuthClientSecret(swaggerSettings.Authentication?.ClientSecret);
			options.OAuthAppName(swaggerSettings.Name);
			options.OAuthUsePkce();
		});
	}

	public static void UseS3Buckets(this IApplicationBuilder app, S3FileProviderConfiguration configuration)
	{
		var service = app.ApplicationServices.GetRequiredService<IS3BucketProvider>();

		if (service is null)
		{
			throw new Exception("Service IS3BucketProvider is not registered");
		}

		var isBucketExist = service.CheckIfBucketExists(configuration.DefaultBucket.Name).GetAwaiter().GetResult();

		if (!isBucketExist)
		{
			service.CreateBucket(configuration.DefaultBucket).GetAwaiter().GetResult();
		}
	}
}
