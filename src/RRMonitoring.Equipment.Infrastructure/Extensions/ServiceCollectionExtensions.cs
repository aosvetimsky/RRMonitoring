using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomium.Core.Data.EntityFrameworkCore.Transactions;
using Nomium.Core.Data.Transactions;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Infrastructure.Database;
using RRMonitoring.Equipment.Infrastructure.Database.Repositories;

namespace RRMonitoring.Equipment.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureReferences(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddDbContext<EquipmentContext>(options =>
			options.UseNpgsql(configuration.GetConnectionString("EquipmentContext")));

		services.AddScoped<ITransactionScopeManager, TransactionScopeManager<EquipmentContext>>();
		services.AddScoped<IEquipmentRepository, EquipmentRepository>();
		services.AddScoped<IEquipmentModelRepository, EquipmentModelRepository>();
		services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
		services.AddScoped<IHashrateUnitRepository, HashrateUnitRepository>();
		services.AddScoped<IFirmwareRepository, FirmwareRepository>();

		return services;
	}
}
