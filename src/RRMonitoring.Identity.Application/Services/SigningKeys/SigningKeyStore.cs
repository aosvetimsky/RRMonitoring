using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.IdentityModel.Tokens;
using RRMonitoring.Identity.Application.Services.SigningKeys.Management;

namespace RRMonitoring.Identity.Application.Services.SigningKeys;

internal class SigningKeyStore : ISigningCredentialStore, IValidationKeysStore
{
	private readonly IKeyManagementService _keyManagementService;
	private readonly string _signingAlgorithmName = IdentityServerConstants.RsaSigningAlgorithm.RS256.ToString();

	public SigningKeyStore(IKeyManagementService keyManagementService)
	{
		_keyManagementService = keyManagementService;
	}

	public async Task<SigningCredentials> GetSigningCredentialsAsync()
	{
		var signingKey = await _keyManagementService.GetCurrentKey();

		return new SigningCredentials(signingKey.ToRsa(), _signingAlgorithmName);
	}

	public async Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
	{
		var allSigningKeys = await _keyManagementService.GetAllKeys();

		return allSigningKeys.Select(key => new SecurityKeyInfo
		{
			Key = key.ToRsa(), SigningAlgorithm = _signingAlgorithmName
		});
	}
}
