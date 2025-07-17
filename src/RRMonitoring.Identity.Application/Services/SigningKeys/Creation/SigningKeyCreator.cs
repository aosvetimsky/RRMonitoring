using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.SigningKeys.Creation;

internal class SigningKeyCreator : ISigningKeyCreator
{
	private readonly ISigningKeyRepository _signingKeyRepository;

	public SigningKeyCreator(ISigningKeyRepository signingKeyRepository)
	{
		_signingKeyRepository = signingKeyRepository;
	}

	public async Task<SigningKey> CreateAndStore()
	{
		var rsa = CreateRsaKey();
		var signingKey = new SigningKey(rsa);

		await _signingKeyRepository.Add(signingKey);

		return signingKey;
	}

	private static RsaSecurityKey CreateRsaKey()
	{
		using var rsa = RSA.Create(2048);

		var key = new RsaSecurityKey(rsa.ExportParameters(true)) { KeyId = Guid.NewGuid().ToString() };

		return key;
	}
}
