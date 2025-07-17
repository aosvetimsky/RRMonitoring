using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomium.Core.ApiClient.Configuration;
using Nomium.Core.ApiClient.Extensions;
using Nomium.Core.ApiClient.Handlers;
using Nomium.Core.Application.Extensions;
using Nomium.Core.Application.Services.DateTimeProvider;
using Nomium.Core.Security.Services.Account;
using Nomium.Core.Security.Services.CurrentUser;
using Nomium.Core.Security.Services.CurrentUser.Models;
using RRMonitoring.Bff.Admin.Application.Configurations;
using RRMonitoring.Bff.Admin.Application.Services.Coins;
using RRMonitoring.Bff.Admin.Application.Services.Equipment;
using RRMonitoring.Bff.Admin.Application.Services.EquipmentModels;
using RRMonitoring.Bff.Admin.Application.Services.Facilities;
using RRMonitoring.Bff.Admin.Application.Services.Firmware;
using RRMonitoring.Bff.Admin.Application.Services.HashrateUnits;
using RRMonitoring.Bff.Admin.Application.Services.Manufacturers;
using RRMonitoring.Bff.Admin.Application.Services.Pools;
using RRMonitoring.Bff.Admin.Application.Services.Users;
using RRMonitoring.Bff.Admin.Application.Services.Workers;
using RRMonitoring.Colocation.ApiClients;
using RRMonitoring.Equipment.ApiClients;
using RRMonitoring.Identity.ApiClients.ApiClients;
using RRMonitoring.Mining.ApiClients;

namespace RRMonitoring.Bff.Admin.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationReferences(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddScoped<ICurrentUserService<CurrentUserBase>, CurrentUserService<CurrentUserBase>>();
		services.AddScoped<IAccountService, AccountService>();

		return services
			.AddApiClients(configuration)
			.AddServicesReferences(configuration);
	}

	private static IServiceCollection AddServicesReferences(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddScoped<IUserService, UserService>();
		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
		services.AddScoped<JwtTokenHttpMessageHandler>();
		services.AddScoped<IPoolService, PoolService>();
		services.AddScoped<IManufacturerService, ManufacturerService>();
		services.AddScoped<IEquipmentModelService, EquipmentModelService>();
		services.AddScoped<IEquipmentService, EquipmentService>();
		services.AddScoped<ICoinService, CoinService>();
		services.AddScoped<IHashrateUnitService, HashrateUnitService>();
		services.AddScoped<IFirmwareService, FirmwareService>();
		services.AddScoped<IFacilityService, FacilityService>();
		services.AddScoped<IWorkerService, WorkerService>();

		return services;
	}

	private static IServiceCollection AddApiClients(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddScoped<UserInfoHeaderHandler>();

		var webApiClientConfigurations =
			configuration.GetRequiredSectionValue<ApiClientConfigurations>("WebApiClients");

		return services
			.AddColocationApiClients(webApiClientConfigurations.ColocationService)
			.AddEquipmentApiClients(webApiClientConfigurations.EquipmentService)
			.AddIdentityApiClients(webApiClientConfigurations.IdentityService)
			.AddMiningApiClients(webApiClientConfigurations.MiningService);
	}

	private static IServiceCollection AddColocationApiClients(
		this IServiceCollection services,
		ApiClientConfiguration colocationConfiguration)
	{
		return services
			.AddRefitClient<IFacilityApiClient>(colocationConfiguration);
	}

	private static IServiceCollection AddEquipmentApiClients(
		this IServiceCollection services,
		ApiClientConfiguration equipmentConfiguration)
	{
		return services
			.AddRefitClient<IManufacturerApiClient>(equipmentConfiguration)
			.AddRefitClient<IEquipmentModelApiClient>(equipmentConfiguration)
			.AddRefitClient<IHashrateUnitApiClient>(equipmentConfiguration)
			.AddRefitClient<IFirmwareApiClient>(equipmentConfiguration);
	}

	private static IServiceCollection AddIdentityApiClients(
		this IServiceCollection services,
		ApiClientConfiguration identityConfiguration)
	{
		return services
			.AddRefitClientWithUserInfoHandler<IUserInternalApiClient>(identityConfiguration);
	}

	private static IServiceCollection AddMiningApiClients(
		this IServiceCollection services,
		ApiClientConfiguration miningConfiguration)
	{
		return services
				.AddRefitClient<IPoolApiClient>(miningConfiguration)
				.AddRefitClient<ICoinApiClient>(miningConfiguration)
				.AddRefitClient<IWorkerApiClient>(miningConfiguration);
	}
}
