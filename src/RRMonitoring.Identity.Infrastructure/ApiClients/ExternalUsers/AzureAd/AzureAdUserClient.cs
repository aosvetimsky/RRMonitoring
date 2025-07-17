using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Web;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Domain.Configuration;
using RRMonitoring.Identity.Domain.Contracts.ExternalProviders;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Infrastructure.ApiClients.ExternalUsers.AzureAd;

public class AzureAdUserClient : IExternalUserClient
{
	private readonly ILogger<AzureAdUserClient> _logger;

	private readonly AzureAdConfiguration _options;

	public AzureAdUserClient(
		IOptions<AzureAdConfiguration> options,
		ILogger<AzureAdUserClient> logger)
	{
		_logger = logger;

		_options = options.Value;
	}

	public string GetUserId(IEnumerable<Claim> claims)
	{
		return claims.FirstOrDefault(x => x.Type == ClaimConstants.ObjectId)?.Value;
	}

	public bool IsUserRegistrationEnabled()
	{
		return _options.IsUserRegistrationEnabled;
	}

	[SuppressMessage("Design", "CA1031:Do not catch general exception types")] // TODO: Catch specific exception
	public async Task<ExternalUser> GetUser(string userId)
	{
		try
		{
			var graphServiceClient = GetGraphClient();

			// We need User.Read.All Application permission in Azure AD to call this method
			var user = await graphServiceClient.Users[userId].GetAsync();
			if (user is null)
			{
				throw new ValidationException("Error when retrieve user information");
			}

			return new ExternalUser
			{
				Id = user.Id,
				FirstName = user.GivenName,
				LastName = user.Surname,
				UserName = GetUserName(user),
				Email = user.Mail,
				PhoneNumber = user.MobilePhone
			};
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Error on getting user from Azure AD, message: {Message}", e.Message);
		}

		return null;
	}

	private static string GetUserName(User user)
	{
		var userName = user.UserPrincipalName?.Split('@').FirstOrDefault();

		if (string.IsNullOrWhiteSpace(userName))
		{
			userName = !string.IsNullOrWhiteSpace(user.Mail) ? user.Mail : user.MobilePhone;
		}

		return userName;
	}

	private GraphServiceClient GetGraphClient()
	{
		var scopes = new[] { "https://graph.microsoft.com/.default" };

		var clientSecretOptions = new ClientSecretCredentialOptions
		{
			AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
		};

		var clientSecretCredential = new ClientSecretCredential(
			_options.TenantId, _options.ClientId, _options.ClientSecret, clientSecretOptions);

		return new GraphServiceClient(clientSecretCredential, scopes);
	}
}
