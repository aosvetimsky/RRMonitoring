using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Nomium.Core.Application.Extensions;
using Nomium.Core.Application.Services.DateTimeProvider;
using Nomium.Core.MediatR.Extensions;
using Nomium.Core.Security.Services.Account;
using Nomium.Core.Security.Services.CurrentUser;
using Nomium.Core.Security.Services.CurrentUser.Models;
using PhoneNumbers;
using RRMonitoring.Identity.Application.Services.Agreement;
using RRMonitoring.Identity.Application.Services.ExternalProviders;
using RRMonitoring.Identity.Application.Services.ForgotPassword;
using RRMonitoring.Identity.Application.Services.Link;
using RRMonitoring.Identity.Application.Services.NotificationHistory;
using RRMonitoring.Identity.Application.Services.Permissions;
using RRMonitoring.Identity.Application.Services.Registration;
using RRMonitoring.Identity.Application.Services.TwoFactor;
using RRMonitoring.Identity.Application.Services.UserValidation;
using RRMonitoring.Identity.Application.Validators;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Notification.ApiClients.Configuration;
using RRMonitoring.Notification.ApiClients.Extensions;

namespace RRMonitoring.Identity.Application.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApplicationReferences(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddMediatRCore(nameof(RRMonitoring));

		services.AddSingleton(PhoneNumberUtil.GetInstance());

		services
			.AddScoped<IAccountService, AccountService>()
			.AddScoped<ICurrentUserService<CurrentUserBase>, CurrentUserService<CurrentUserBase>>()
			.AddScoped<ILinkService, LinkService>()
			.AddScoped<IExternalProviderService, ExternalProviderService>()
			.AddScoped<IForgotPasswordService, ForgotPasswordService>()
			.AddScoped<IUserRegistrationService, UserRegistrationService>()
			.AddScoped<IIdentityNotificationHistoryService, IdentityNotificationHistoryService>()
			.AddScoped<ITwoFactorService, TwoFactorService>()
			.AddScoped<IUserAccessVerificationService, UserAccessVerificationService>();

		services.AddScoped<IPermissionValidator, PermissionValidator>();
		services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

		services.AddScoped(_ => CryptoProviderFactory.Default);
		services.AddScoped<IVerifiedLoginService, VerifiedLoginService>();

		services.AddTransient<IUserValidator<User>, EmailUserValidator>();

		var notificationHttpConfig = configuration.GetRequiredSectionValue<NotificationHttpConfiguration>("NotificationService");
		services.RegisterHttpNotificationManager(notificationHttpConfig);
		services.RegisterRabbitNotificationManager();
		services.RegisterNotificationApiServices();

		return services;
	}
}
