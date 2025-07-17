using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.OpenApi.Models;
using Nomium.Core.Security.Configuration;
using Nomium.Core.Swagger.Configuration;
using Nomium.Core.Swagger.Filters;
using Swashbuckle.AspNetCore.Swagger;

namespace RRMonitoring.Identity.Api.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddIdentitySwagger(this IServiceCollection services, IConfiguration configuration)
	{
		var identitySettings = configuration.GetSection("Identity").Get<IdentitySettings>();
		var swaggerSettings = configuration.GetSection("Swagger").Get<SwaggerSettings>();

		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1",
				new OpenApiInfo { Title = "Identity", Version = "v1", Description = "Identity Server API" });

			options.AddSecurityDefinition("oauth2",
				new OpenApiSecurityScheme
				{
					Type = SecuritySchemeType.OAuth2,
					In = ParameterLocation.Header,
					Flows = new OpenApiOAuthFlows
					{
						AuthorizationCode = new OpenApiOAuthFlow
						{
							AuthorizationUrl = new Uri(swaggerSettings.Identity.AuthorizationUrl),
							TokenUrl = new Uri(swaggerSettings.Identity.TokenUrl),
							Scopes = new Dictionary<string, string>
							{
								{ "openid", "openid" },
								{ identitySettings.ApiName, identitySettings.ApiName }
							}
						}
					}
				});

			options.OperationFilter<SwaggerAuthorizationOperationFilter>();
			options.AddFluentValidationRulesScoped();
		});

		return services;
	}

	public static IServiceCollection AddAutoMapper(
		this IServiceCollection services, string prefixAssemblyName,
		Action<IMapperConfigurationExpression> configuration = null)
	{
		var mapper = new Mapper(new MapperConfiguration(ctx =>
		{
			var assemblies = DependencyContext.Default.RuntimeLibraries
				.SelectMany(assembly => assembly.GetDefaultAssemblyNames(DependencyContext.Default)
					.Where(assemblyName => assemblyName.FullName.StartsWith(prefixAssemblyName))
					.Select(Assembly.Load))
				.ToArray();

			ctx.AllowNullCollections = true;

			ctx.AddMaps(assemblies);
			ctx.AddCollectionMappers();

			configuration?.Invoke(ctx);
		}));

		services.AddSingleton<IMapper>(mapper);

		return services;
	}
}
