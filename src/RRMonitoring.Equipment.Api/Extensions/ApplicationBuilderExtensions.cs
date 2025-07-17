using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nomium.Core.FileStorage.Providers.S3;
using Nomium.Core.FileStorage.Providers.S3.Configuration;

namespace RRMonitoring.Equipment.Api.Extensions;

public static class ApplicationBuilderExtensions
{
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
