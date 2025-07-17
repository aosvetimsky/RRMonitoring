using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RRMonitoring.Identity.Infrastructure.Database;

namespace RRMonitoring.Identity.Infrastructure.Extensions;

public static class IdentityServerBuilderExtensions
{
	public static IIdentityServerBuilder AddConfigurationInDatabase(this IIdentityServerBuilder builder, string connectionString)
	{
		var migrationsAssembly = typeof(IdentityServerBuilderExtensions).GetTypeInfo().Assembly.GetName().Name;

		builder.AddConfigurationStore<IdentityContext>(options =>
		{
			options.ConfigureDbContext = optionsBuilder =>
				optionsBuilder.UseNpgsql(connectionString,
					npgsqlBuilder => npgsqlBuilder.MigrationsAssembly(migrationsAssembly));
		});

		return builder;
	}

	public static IIdentityServerBuilder AddOperationalInDatabase(this IIdentityServerBuilder builder, string connectionString)
	{
		var migrationsAssembly = typeof(IdentityServerBuilderExtensions).GetTypeInfo().Assembly.GetName().Name;

		builder.AddOperationalStore<IdentityContext>(options =>
		{
			options.ConfigureDbContext = optionsBuilder =>
				optionsBuilder.UseNpgsql(connectionString,
					npgsqlBuilder => npgsqlBuilder.MigrationsAssembly(migrationsAssembly));

			options.EnableTokenCleanup = true;
		});

		return builder;
	}

	public static IdentityBuilder AddIdentityContextStore(this IdentityBuilder builder)
	{
		return builder.AddEntityFrameworkStores<IdentityContext>();
	}
}
