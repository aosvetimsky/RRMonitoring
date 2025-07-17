using System;
using System.IO;
using System.Security.Claims;
using FluentValidation.AspNetCore;
using IdentityServer4;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Nomium.Core.Application.Extensions;
using Nomium.Core.FileStorage.Extensions;
using Nomium.Core.MassTransit.Configuration;
using Nomium.Core.MassTransit.Extensions;
using Nomium.Core.Security.Extensions;
using Nomium.Core.Security.Services.CurrentUser.Models;
using Nomium.Core.Swagger.Configuration;
using Nomium.Core.Swagger.Extensions;
using Nomium.Core.Swagger.Extensions.DependencyInjection;
using Prometheus;
using Prometheus.SystemMetrics;
using RRMonitoring.Identity.Api.Extensions;
using RRMonitoring.Identity.Api.Filters;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Extensions;
using RRMonitoring.Identity.Application.Services.ClaimsProfile;
using RRMonitoring.Identity.Application.Services.YandexSmartCaptcha;
using RRMonitoring.Identity.Application.Validators;
using RRMonitoring.Identity.Domain.Configuration;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Infrastructure.Extensions;

namespace RRMonitoring.Identity.Api;

public class Startup
{
	private const string AssemblyPrefix = "RRMonitoring.Identity";
	private const string CorsName = nameof(Identity);
	private const string S3SectionName = "S3FileProvider";

	private readonly IConfiguration _configuration;
	private readonly IWebHostEnvironment _environment;

	public Startup(
		IConfiguration configuration,
		IWebHostEnvironment environment)
	{
		_configuration = configuration;
		_environment = environment;
	}

	public void ConfigureServices(IServiceCollection services)
	{
		if (_environment.IsTestEnvironment())
		{
			services.AddCors(o => o.AddPolicy(CorsName, options =>
			{
				options.SetIsOriginAllowed(_ => true)
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowCredentials();
			}));
		}

		services.AddMvc(options =>
		{
			options.Filters.Add<MvcGlobalExceptionsFilter>();
		});

		var swaggerSettings = _configuration.GetSection("Swagger").Get<SwaggerSettings>();
		services.AddSwaggerCore(swaggerSettings);

		services.AddIdentityCore<User>()
			.AddIdentityManagementServices()
			.AddIdentityContextStore()
			.AddDefaultTokenProviders()
			.AddPasswordValidator<SameCharactersInLoginAndPasswordValidator>();

		var rabbitMqConfig = _configuration.GetRequiredSectionValue<RabbitMqConfiguration>("RabbitMq");
		services.AddMassTransitRabbitMq(rabbitMqConfig);

		services.AddInfrastructureReferences(_configuration);
		services.AddApplicationReferences(_configuration);

		services.Configure<IdentityS3FileProviderConfiguration>(options =>
			_configuration.GetSection(S3SectionName).Bind(options));
		var s3Configuration = _configuration.GetRequiredSectionValue<IdentityS3FileProviderConfiguration>(S3SectionName);
		services.AddS3FileProvider(s3Configuration);

		services.AddAutoMapper(nameof(RRMonitoring));

		services.AddFluentValidationAutoValidation();

		services.Configure<IdentityOptions>(options => _configuration.GetSection("IdentityServer").Bind(options));
		services.Configure<RedRockIdentityOptions>(options => _configuration.GetSection("IdentityServer").Bind(options));
		services.Configure<RegexesConfig>(options => _configuration.GetSection("Regexes").Bind(options));
		services.Configure<AuthenticationConfig>(options => _configuration.GetSection("AuthenticationConfig").Bind(options));
		services.Configure<ResetPasswordConfig>(options => _configuration.GetSection("ResetPasswordConfig").Bind(options));
		services.Configure<TimeoutConfig>(options => _configuration.GetSection("TimeoutConfig").Bind(options));
		services.Configure<CacheConfiguration>(options => _configuration.GetSection("CacheConfig").Bind(options));
		services.Configure<DefaultRedirectUrlsConfiguration>(options =>
			_configuration.GetSection("DefaultRedirectUrls").Bind(options));

		services.Configure<YandexSmartCaptchaConfiguration>(options => _configuration.GetSection("YandexSmartCaptchaConfig").Bind(options));
		services.AddScoped<YandexSmartCaptchaService>();

		var tokenLifespan = _configuration.GetValue<TimeSpan>("IdentityServer:TokenLifespan");
		services.Configure<DataProtectionTokenProviderOptions>(options =>
		{
			options.TokenLifespan = tokenLifespan;
		});

		var identityBuilder = services.AddIdentityServer(options =>
			{
				options.UserInteraction.LoginUrl = "/login";
				options.UserInteraction.LogoutUrl = "/logout";
				options.UserInteraction.ErrorUrl = "/error";
			})
			.AddConfigurationInDatabase(_configuration.GetConnectionString("IdentityContext"))
			.AddOperationalInDatabase(_configuration.GetConnectionString("IdentityContext"))
			.AddAspNetIdentity<User>()
			.AddProfileService<ClaimsProfileService>()
			.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
			.AddExtensionGrantValidator<TwoFactorGrantTypeValidator>();

		if (_environment.IsDevelopment())
		{
			identityBuilder.AddDeveloperSigningCredential();
		}
		else
		{
			identityBuilder.AddSigningKeyManagement(_configuration);
		}

		var issuerUri = _configuration.GetValue<string>("IdentityServer:IssuerUri");

		var authBuilder = services
			.AddAuthentication()
			.AddOpenIdConnect(options =>
			{
				options.SignInScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
				options.SignOutScheme = IdentityServerConstants.SignoutScheme;
				options.SaveTokens = true;

				options.ClientId = "identity";
				options.ClientSecret = "secret";

				options.Authority = issuerUri;
				options.RequireHttpsMetadata = _environment.IsProduction();
				options.UsePkce = false;

				options.TokenValidationParameters = new TokenValidationParameters
				{
					RoleClaimType = ClaimTypes.Role
				};
			})
			.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
			{
				options.Authority = _configuration.GetValue<string>("Identity:IdentityServerUrl");
				options.Audience = _configuration.GetValue<string>("Identity:ApiName");
				options.RequireHttpsMetadata = _environment.IsProduction();

				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateLifetime = true,
					RequireExpirationTime = true,
					ValidateAudience = true,
					ClockSkew = TimeSpan.Zero,
				};
			})
			.AddApiKeySupport(options =>
			{
				options.ApiKeyValue = _configuration.GetRequiredSectionValue<string>("ApiKey");
			});

		authBuilder.AddIdentityCookies();

		services.Configure<ActiveDirectoryConfiguration>(options => _configuration.GetSection("ActiveDirectories").Bind(options));
		var adConfiguration = _configuration.GetSection("ActiveDirectories").Get<ActiveDirectoryConfiguration>();
		if (adConfiguration.AzureAd?.IsEnabled == true)
		{
			services.Configure<AzureAdConfiguration>(options =>
				_configuration.GetSection("ActiveDirectories:AzureAd").Bind(options));

			authBuilder.AddAzureAd(adConfiguration.AzureAd);
		}

		services.Configure<ReferralCodeSettingsConfiguration>(_configuration.GetSection("ReferralCodeSettings"));

		services.AddAuthorization(options =>
		{
			options.AddPermissionPolicies();
		});

		services.AddHealthChecks();

		services.AddSystemMetrics();
		services.AddApplicationMetrics();

		services.AddOpenTelemetryReferences(AssemblyPrefix);
	}

	public void Configure(IApplicationBuilder app)
	{
		var issuerUri = _configuration.GetValue<string>("IdentityServer:IssuerUri");
		app.Use(async (context, next) =>
		{
			context.SetIdentityServerOrigin(issuerUri);
			await next();
		});

		var pathBase = _configuration.GetValue<string>("PathBase");
		app.UsePathBase(pathBase);

		if (_environment.IsTestEnvironment())
		{
			app.UseDeveloperExceptionPage();
			app.UseCors(CorsName);

			var swaggerSettings = _configuration.GetSection("Swagger").Get<SwaggerSettings>();
			app.UseSwaggerBase(swaggerSettings);
		}

		app.UseCookiePolicy(new CookiePolicyOptions
		{
			MinimumSameSitePolicy = _environment.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.None,
			Secure = _environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always
		});

		// app.UseHttpsRedirection();

		app.UseRouting();

		app.UseExceptionWithErrorCodeMiddleware();

		var frontendAssetsPath = _configuration.GetRequiredSectionValue<string>("FrontendAssetsPath");
		app.UseStaticFiles(new StaticFileOptions
		{
			FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), frontendAssetsPath)),
			RequestPath = new PathString("/assets")
		});

		var s3Configuration = _configuration.GetRequiredSectionValue<IdentityS3FileProviderConfiguration>(S3SectionName);
		app.UseS3Buckets(s3Configuration);

		app.UseIdentityServer();
		app.UseAuthentication();
		app.UseAuthorization();

		app.UseCurrentUserMiddleware();
		app.UseInternalCurrentUserMiddleware<CurrentUserBase>();

		app.UseHealthChecks("/health");

		app.UseMetricServer();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers();
		});
	}
}
