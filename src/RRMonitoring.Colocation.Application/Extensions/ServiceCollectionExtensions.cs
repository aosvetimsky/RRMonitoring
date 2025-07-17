using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nomium.Core.Application.Services.DateTimeProvider;
using Nomium.Core.Security.Services.CurrentUser;
using Nomium.Core.Security.Services.CurrentUser.Models;

namespace RRMonitoring.Colocation.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationReferences(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

		services.AddScoped<ICurrentUserService<CurrentUserBase>, CurrentUserService<CurrentUserBase>>();

		return services;
	}
}
