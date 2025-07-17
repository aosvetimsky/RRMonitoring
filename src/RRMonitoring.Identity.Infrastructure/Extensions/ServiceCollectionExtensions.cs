using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RRMonitoring.Identity.Domain.Contracts;
using RRMonitoring.Identity.Domain.Contracts.ExternalProviders;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Infrastructure.ApiClients.ExternalUsers;
using RRMonitoring.Identity.Infrastructure.ApiClients.ExternalUsers.AzureAd;
using RRMonitoring.Identity.Infrastructure.Database;
using RRMonitoring.Identity.Infrastructure.Database.IdentityStores;
using RRMonitoring.Identity.Infrastructure.Database.Repositories;
using RRMonitoring.Identity.Infrastructure.Transactions;
using User = RRMonitoring.Identity.Domain.Entities.User;

namespace RRMonitoring.Identity.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureReferences(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddEntityFrameworkNpgsql()
			.AddDbContext<IdentityContext>(opt =>
				opt.UseNpgsql(configuration.GetConnectionString("IdentityContext")));

		services
			.AddScoped<ISigningKeyRepository, SigningKeyRepository>()
			.AddScoped<IUserRepository, UserRepository>()
			.AddScoped<IRoleRepository, RoleRepository>()
			.AddScoped<IScopeRepository, ScopeRepository>()
			.AddScoped<IPermissionRepository, PermissionRepository>()
			.AddScoped<IPermissionGrantRepository, PermissionGrantRepository>()
			.AddScoped<ITenantRepository, TenantRepository>()
			.AddScoped<IUserTypeRepository, UserTypeRepository>()
			.AddScoped<IExternalSourceRepository, ExternalSourceRepository>()
			.AddScoped<ICountryRepository, CountryRepository>()
			.AddScoped<ITransactionScopeManager, TransactionScopeManager>()
			.AddScoped<IUserStore<User>, IdentityUserStore>()
			.AddScoped<IUserStatusRepository, UserStatusRepository>()
			.AddScoped<IExternalPermissionRepository, ExternalPermissionRepository>()
			.AddScoped<IRoleStore<Role>, RoleStore<Role, IdentityContext, Guid>>();

		services.AddExternalUserClients();

		return services;
	}

	public static IServiceCollection AddOpenTelemetryReferences(
		this IServiceCollection services,
		string serviceName)
	{
		services.AddOpenTelemetry()
			.ConfigureResource(resource => resource
				.AddService(serviceName: serviceName))
			.WithTracing(tracing => tracing
				.AddAspNetCoreInstrumentation()
				.AddHttpClientInstrumentation()
				.AddEntityFrameworkCoreInstrumentation()
				.AddMassTransitInstrumentation()
				.AddSqlClientInstrumentation()
				.AddRedisInstrumentation()
				.AddOtlpExporter());

		return services;
	}

	private static IServiceCollection AddExternalUserClients(this IServiceCollection services)
	{
		services.AddScoped<IExternalUserFactory, ExternalUserFactory>();

		services.AddScoped<AzureAdUserClient>();
		services.AddScoped<IExternalUserClient, AzureAdUserClient>(x =>
			x.GetService<AzureAdUserClient>());

		return services;
	}
}
