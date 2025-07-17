using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RRMonitoring.Identity.Application.Services.RoleManager;
using RRMonitoring.Identity.Application.Services.SigningKeys;
using RRMonitoring.Identity.Application.Services.SigningKeys.Cache;
using RRMonitoring.Identity.Application.Services.SigningKeys.Creation;
using RRMonitoring.Identity.Application.Services.SigningKeys.Management;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Extensions;

public static class IdentityServerBuilderExtensions
{
	public static IIdentityServerBuilder AddSigningKeyManagement(this IIdentityServerBuilder builder, IConfiguration configuration)
	{
		builder.Services.Configure<SigningKeysSettings>(configuration.GetSection("IdentityServer:SigningKeys"));

		builder.Services
			.AddScoped<ISigningCredentialStore, SigningKeyStore>()
			.AddScoped<IValidationKeysStore, SigningKeyStore>()
			.AddScoped<IKeyManagementService, KeyManagementService>()
			.AddScoped<ISigningKeyCreator, SigningKeyCreator>()
			.AddScoped<ISigningKeyCache, SigningKeyCache>();

		return builder;
	}

	public static IdentityBuilder AddIdentityManagementServices(this IdentityBuilder builder)
	{
		return builder
			.AddRoles<Role>()
			.AddUserManager<IdentityUserManager>()
			.AddRoleManager<IdentityRoleManager>()
			.AddSignInManager<SignInManager<User>>()
			.AddIdentityRoles();
	}

	private static IdentityBuilder AddIdentityRoles(this IdentityBuilder builder)
	{
		builder.Services
			.AddScoped<IRoleValidator<Role>, IdentityRoleValidator>()
			.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User, Role>>();

		return builder;
	}
}
