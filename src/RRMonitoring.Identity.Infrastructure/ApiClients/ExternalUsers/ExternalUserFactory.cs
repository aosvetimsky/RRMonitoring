using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Domain.Constants;
using RRMonitoring.Identity.Domain.Contracts.ExternalProviders;
using RRMonitoring.Identity.Domain.Models;
using RRMonitoring.Identity.Infrastructure.ApiClients.ExternalUsers.AzureAd;

namespace RRMonitoring.Identity.Infrastructure.ApiClients.ExternalUsers;

internal class ExternalUserFactory : IExternalUserFactory
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<ExternalUserFactory> _logger;

	public ExternalUserFactory(
		IServiceProvider serviceProvider,
		ILogger<ExternalUserFactory> logger)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	public string GetUserExternalId(string provider, IEnumerable<Claim> claims)
	{
		var externalUserClient = GetExternalUserClient(provider);

		return externalUserClient.GetUserId(claims);
	}

	public bool IsUserRegistrationEnabled(string provider)
	{
		var externalUserClient = GetExternalUserClient(provider);

		return externalUserClient.IsUserRegistrationEnabled();
	}

	public async Task<ExternalUser> GetByProvider(string provider, string externalUserId)
	{
		var externalUserClient = GetExternalUserClient(provider);
		var externalUser = await externalUserClient.GetUser(externalUserId);
		if (externalUser is null)
		{
			_logger.LogError("No external user for provider: '{Provider}', and ID: '{ExternalUserId}'", provider, externalUserId);

			throw new ValidationException($"No external user for provider: {provider}, and ID: {externalUserId}");
		}

		return externalUser;
	}

	private IExternalUserClient GetExternalUserClient(string provider)
	{
		return provider switch
		{
			AzureAdDefaults.AuthenticationScheme => (IExternalUserClient)_serviceProvider.GetService(typeof(AzureAdUserClient)),
			_ => throw new NotImplementedException($"No external provider with Name: {provider}")
		};
	}
}
