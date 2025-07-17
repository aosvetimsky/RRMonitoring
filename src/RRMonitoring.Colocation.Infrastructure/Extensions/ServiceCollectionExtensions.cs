using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RRMonitoring.Colocation.Domain.Contracts.Repositories;
using RRMonitoring.Colocation.Infrastructure.Database;
using RRMonitoring.Colocation.Infrastructure.Database.Repositories;

namespace RRMonitoring.Colocation.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureReferences(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddDbContext<ColocationContext>(options =>
			options.UseNpgsql(configuration.GetConnectionString("ColocationContext")));

		services.AddScoped<IFacilityRepository, FacilityRepository>();

		return services;
	}
}
