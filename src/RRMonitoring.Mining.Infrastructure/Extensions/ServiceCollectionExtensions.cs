using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RRMonitoring.Mining.Domain.Contracts.Repositories;
using RRMonitoring.Mining.Infrastructure.Database;
using RRMonitoring.Mining.Infrastructure.Database.Repositories;

namespace RRMonitoring.Mining.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureReferences(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddDbContext<MiningContext>(options =>
			options.UseNpgsql(configuration.GetConnectionString("MiningContext")));

		services.AddScoped<IWorkerRepository, WorkerRepository>();
		services.AddScoped<IPoolRepository, PoolRepository>();
		services.AddScoped<ICoinRepository, CoinRepository>();

		return services;
	}
}
