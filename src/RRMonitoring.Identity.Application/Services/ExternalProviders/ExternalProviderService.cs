using System.Collections.Generic;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Services.ExternalProviders.Models;
using RRMonitoring.Identity.Domain.Configuration;
using RRMonitoring.Identity.Domain.Constants;

namespace RRMonitoring.Identity.Application.Services.ExternalProviders;

public class ExternalProviderService : IExternalProviderService
{
	private readonly ActiveDirectoryConfiguration _activeDirectoryOptions;

	public ExternalProviderService(IOptions<ActiveDirectoryConfiguration> activeDirectoryOptions)
	{
		_activeDirectoryOptions = activeDirectoryOptions.Value;
	}

	public List<LoginProvider> GetActiveDirectoryProviders()
	{
		var providers = new List<LoginProvider>();

		if (IsAzureAdSectionValid(_activeDirectoryOptions.AzureAd))
		{
			providers.Add(new LoginProvider(AzureAdDefaults.AuthenticationScheme,
				_activeDirectoryOptions.AzureAd.DisplayName, _activeDirectoryOptions.AzureAd.IconUrl));
		}

		return providers;
	}

	private static bool IsAzureAdSectionValid(AzureAdConfiguration azureAdConfiguration)
	{
		return azureAdConfiguration is not null
		       && azureAdConfiguration.IsEnabled
		       && !string.IsNullOrWhiteSpace(azureAdConfiguration.TenantId)
		       && !string.IsNullOrWhiteSpace(azureAdConfiguration.ClientId)
		       && !string.IsNullOrWhiteSpace(azureAdConfiguration.ClientSecret);
	}
}
