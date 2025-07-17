using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RRMonitoring.Notification.Infrastructure.Extensions;
using Serilog;

namespace RRMonitoring.Notification.Api;

public static class Program
{
	public static async Task Main(string[] args)
	{
		var configuration = GetConfiguration();

		Log.Logger = new LoggerConfiguration()
			.ReadFrom.Configuration(configuration)
			.CreateLogger();

		var host = BuildWebHost(configuration, args);

		await host.MigrateDatabaseSchema();

		host.MigrateDataToDatabase(configuration, Log.Logger);

		await host.RunAsync();
	}

	private static IHost BuildWebHost(IConfiguration configuration, string[] args)
	{
		return Host.CreateDefaultBuilder(args)
			.UseSerilog((context, config) =>
			{
				config.ReadFrom.Configuration(configuration);
			})
			.ConfigureWebHostDefaults(webHostBuilder =>
			{
				webHostBuilder
					.UseConfiguration(configuration)
					.UseStartup<Startup>();
			})
			.Build();
	}

	private static IConfiguration GetConfiguration()
	{
		var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

		var builder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddJsonFile($"appsettings.{environmentName}.json", optional: true)
			.AddEnvironmentVariables();

		return builder.Build();
	}
}
