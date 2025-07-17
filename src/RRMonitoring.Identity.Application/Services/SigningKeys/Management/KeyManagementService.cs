using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Services.SigningKeys.Cache;
using RRMonitoring.Identity.Application.Services.SigningKeys.Creation;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.SigningKeys.Management;

internal class KeyManagementService : IKeyManagementService
{
	private readonly ISigningKeyCreator _signingKeyCreator;
	private readonly ISigningKeyCache _signingKeyCache;
	private readonly ISigningKeyRepository _signingKeyRepository;
	private readonly ILogger<KeyManagementService> _logger;
	private readonly SigningKeysSettings _settings;

	public KeyManagementService(
		ISigningKeyCreator signingKeyCreator,
		ISigningKeyCache signingKeyCache,
		ISigningKeyRepository signingKeyRepository,
		ILogger<KeyManagementService> logger,
		IOptions<SigningKeysSettings> options)
	{
		_signingKeyCreator = signingKeyCreator;
		_signingKeyCache = signingKeyCache;
		_signingKeyRepository = signingKeyRepository;
		_logger = logger;

		_settings = options.Value;
	}

	public async Task<SigningKey> GetCurrentKey()
	{
		_logger.LogDebug("Getting the current key for token signing");

		var (_, activeKey) = await GetNonRetiredAndCurrentKeys();

		return activeKey;
	}

	public async Task<List<SigningKey>> GetAllKeys()
	{
		_logger.LogDebug("Getting all the keys used for token signing (current, expired but not retired, next active)");

		var (allKeys, _) = await GetNonRetiredAndCurrentKeys();

		return allKeys;
	}

	private SigningKey GetCurrentKey(IReadOnlyCollection<SigningKey> keys)
	{
		return GetActiveKeys(keys)
			.OrderBy(key => key.CreationDate)
			.FirstOrDefault();
	}

	private bool IsKeyRotationRequired(IReadOnlyCollection<SigningKey> notRetiredKeys)
	{
		var activeKeys = GetActiveKeys(notRetiredKeys);
		var latestActiveKey = activeKeys.OrderByDescending(k => k.CreationDate).First();

		var timeTillExpiration = latestActiveKey.CreationDate
			.Add(_settings.ExpirationPeriod)
			.Subtract(DateTime.UtcNow);

		return timeTillExpiration < _settings.ActivationPeriod;
	}

	private IEnumerable<SigningKey> GetActiveKeys(IReadOnlyCollection<SigningKey> notRetiredKeys)
	{
		var notExpiredKeysCreationDate = DateTime.UtcNow.Subtract(_settings.ExpirationPeriod);

		return notRetiredKeys.Where(k => k.CreationDate > notExpiredKeysCreationDate).ToList();
	}

	private async ValueTask<(List<SigningKey>, SigningKey)> GetNonRetiredAndCurrentKeys()
	{
		var notRetiredKeys = await GetNonRetiredKeys();

		var currentKey = GetCurrentKey(notRetiredKeys);

		if (currentKey == null)
		{
			_logger.LogInformation("No active key was found. New one will be created");

			return await IntroduceNewKeyAndUpdateCache(notRetiredKeys);
		}

		if (IsKeyRotationRequired(notRetiredKeys))
		{
			_logger.LogInformation("Approaching expiration for key {SigningKeyId}. New one will be created",
				currentKey.Id);

			var (updatedNotRetiredKeys, _) = await IntroduceNewKeyAndUpdateCache(notRetiredKeys);

			return (updatedNotRetiredKeys, currentKey);
		}

		return (notRetiredKeys, currentKey);
	}

	private async ValueTask<List<SigningKey>> GetNonRetiredKeys()
	{
		if (_signingKeyCache.TryGetKeys(out var nonRetiredKeys))
		{
			_logger.LogDebug("Cache hit when loading all non retired keys");

			return nonRetiredKeys!;
		}

		_logger.LogDebug("Cache miss when loading all non retired keys");

		var notRetiredKeys = await GetNotRetiredKeysFromDb();

		if (notRetiredKeys.Any())
		{
			_signingKeyCache.CacheKeys(notRetiredKeys);
		}

		return notRetiredKeys;
	}

	private Task<List<SigningKey>> GetNotRetiredKeysFromDb()
	{
		var retirementDate = DateTime.UtcNow.Subtract(_settings.RetirementPeriod);

		return _signingKeyRepository.Find(x => x.CreationDate > retirementDate);
	}

	private async Task<(List<SigningKey>, SigningKey)> IntroduceNewKeyAndUpdateCache(
		IEnumerable<SigningKey> notRetiredKeys)
	{
		var newKey = await _signingKeyCreator.CreateAndStore();

		var updatedNotRetiredKeys = notRetiredKeys.Append(newKey).ToList();

		_signingKeyCache.CacheKeys(updatedNotRetiredKeys);

		return (updatedNotRetiredKeys, newKey);
	}
}
