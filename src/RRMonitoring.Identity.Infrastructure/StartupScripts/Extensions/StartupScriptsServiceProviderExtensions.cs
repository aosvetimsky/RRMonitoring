using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RRMonitoring.Identity.Infrastructure.StartupScripts.Settings;

namespace RRMonitoring.Identity.Infrastructure.StartupScripts.Extensions;

internal static class StartupScriptsServiceProviderExtensions
{
	public static async Task RunStartupScripts<TDbContext>(
		this IServiceProvider serviceProvider,
		IConfigurationSection configuration,
		CancellationToken cancellationToken
	)
		where TDbContext : DbContext
	{
		var bundleSettings = configuration.Get<StartupScriptBundleSettings>();

		if (!bundleSettings.Enabled)
		{
			return;
		}

		using var scope = serviceProvider.CreateScope();
		var scopeServiceProvider = scope.ServiceProvider;

		var logger = scopeServiceProvider.GetRequiredService<ILogger<StartupScriptBundleExecutor>>();
		var dbContext = scopeServiceProvider.GetRequiredService<TDbContext>();

		var bundleExecutor = new StartupScriptBundleExecutor(logger);

		var sqlQueryExecutor = new NpgsqlDbContextTransactionalSqlQueryExecutor(
			logger,
			dbContext,
			doCommit: !bundleSettings.NoCommit
		);

		await bundleExecutor.ExecuteStartupScripts(sqlQueryExecutor, bundleSettings, cancellationToken);
	}
}
