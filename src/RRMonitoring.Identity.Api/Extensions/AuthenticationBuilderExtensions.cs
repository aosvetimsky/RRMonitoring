using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using RRMonitoring.Identity.Domain.Configuration;
using RRMonitoring.Identity.Domain.Constants;

namespace RRMonitoring.Identity.Api.Extensions;

public static class AuthenticationBuilderExtensions
{
	public static AuthenticationBuilder AddAzureAd(
		this AuthenticationBuilder builder,
		AzureAdConfiguration adOptions)
	{
		builder.AddOpenIdConnect(AzureAdDefaults.AuthenticationScheme, "Azure AD",
			options =>
			{
				options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
				options.SignOutScheme = IdentityServerConstants.SignoutScheme;
				options.ResponseType = OpenIdConnectResponseType.Code;

				options.Authority = $"https://login.windows.net/{adOptions.TenantId}";
				options.ClientId = adOptions.ClientId;
				options.ClientSecret = adOptions.ClientSecret;
				options.CallbackPath = "/signin-azure-ad";
			});

		return builder;
	}
}
