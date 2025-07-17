using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RRMonitoring.Identity.Infrastructure.Extensions;
using Serilog;
using ILogger = Serilog.ILogger;

namespace RRMonitoring.Identity.Api;

public static class Program
{
	public static async Task<int> Main(string[] args)
	{
		var configuration = GetConfiguration();

		Log.Logger = CreateSerilogLogger(configuration);

		try
		{
			Log.Information("Starting up Identity");
			var host = BuildWebHost(configuration, args);

			Log.Information("Migrating databases web host Identity API");

			await host.MigrateDatabaseSchema();

			host.MigrateDataToDatabase(configuration, Log.Logger);

			await host.RunStartupScripts(configuration);

			await host.RunAsync();

			return 0;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);

			throw;
		}
	}

	private static IHost BuildWebHost(IConfiguration configuration, string[] args)
	{
		return Host.CreateDefaultBuilder(args)
			.ConfigureLogging(loggingBuilder =>
			{
				loggingBuilder.ClearProviders();
			})
			.UseSerilog((context, config) =>
			{
				config
					.ReadFrom.Configuration(context.Configuration);
			})
			.ConfigureWebHostDefaults(webHostBuilder =>
			{
				webHostBuilder
					.UseContentRoot(Directory.GetCurrentDirectory())
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

	private static ILogger CreateSerilogLogger(IConfiguration configuration)
	{
		return new LoggerConfiguration()
			.ReadFrom.Configuration(configuration)
			.CreateLogger();
	}
}
