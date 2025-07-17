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
using RRMonitoring.Notification.Infrastructure.Database;
using Serilog;

namespace RRMonitoring.Notification.Infrastructure.Extensions;

public static class HostExtensions
{
	public static Task MigrateDatabaseSchema(this IHost host)
	{
		return host.MigrateDatabase<NotificationContext>(CancellationToken.None);
	}

	public static void MigrateDataToDatabase(
		this IHost host,
		IConfiguration configuration,
		ILogger logger)
	{
		try
		{
			using var connection = new NpgsqlConnection(configuration.GetConnectionString("NotificationContext"));

			var evolve = new Evolve(connection, logger.Information)
			{
				Locations = [configuration.GetSection("Evolve")["DataMigrationsPath"]],
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
}
