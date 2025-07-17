using System;
using System.Threading;
using System.Threading.Tasks;
using EvolveDb;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nomium.Core.Application.Extensions;
using Nomium.Core.Data.EntityFrameworkCore.Extensions;
using Npgsql;
using RRMonitoring.Identity.Infrastructure.Database;
using RRMonitoring.Identity.Infrastructure.StartupScripts.Extensions;
using Serilog;

namespace RRMonitoring.Identity.Infrastructure.Extensions;

public static class HostExtensions
{
	public static Task MigrateDatabaseSchema(this IHost host)
	{
		return host.MigrateDatabase<IdentityContext>(CancellationToken.None);
	}

	public static void MigrateDataToDatabase(
		this IHost host,
		IConfiguration configuration,
		ILogger logger)
	{
		try
		{
			using var connection = new NpgsqlConnection(configuration.GetConnectionString("IdentityContext"));

			var evolve = new Evolve(connection)
			{
				Locations = new[] { configuration.GetSection("Evolve")["DataMigrationsPath"] },
				IsEraseDisabled = true
			};

			if (host.Services.GetRequiredService<IWebHostEnvironment>().IsTestEnvironment())
			{
				evolve.Repair();
			}

			evolve.Migrate();
		}
		catch (Exception ex)
		{
			logger.Fatal(ex, "Database migration failed");
			throw;
		}
	}

	public static async Task RunStartupScripts(this IHost host, IConfiguration configuration)
	{
		var startupScriptsConfiguration = configuration.GetRequiredSection("StartupScripts");

		await host.Services.RunStartupScripts<IdentityContext>(startupScriptsConfiguration, CancellationToken.None);
	}
}
