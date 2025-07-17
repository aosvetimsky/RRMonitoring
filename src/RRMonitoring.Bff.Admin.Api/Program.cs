using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

namespace RRMonitoring.Bff.Admin.Api;

public static class Program
{
	public static async Task Main(string[] args)
	{
		Console.OutputEncoding = Encoding.UTF8;

		var configuration = GetConfiguration();

		Log.Logger = CreateSerilogLogger(configuration);

		var host = BuildWebHost(configuration, args);

		await host.RunAsync();
	}

	private static IHost BuildWebHost(IConfiguration configuration, string[] args)
	{
		return Host.CreateDefaultBuilder(args)
			.UseSerilog((_, config) =>
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
			.AddUserSecrets(typeof(Program).Assembly)
			.AddEnvironmentVariables();

		return builder.Build();
	}

	private static Logger CreateSerilogLogger(IConfiguration configuration)
	{
		return new LoggerConfiguration()
			.ReadFrom.Configuration(configuration)
			.CreateLogger();
	}
}
