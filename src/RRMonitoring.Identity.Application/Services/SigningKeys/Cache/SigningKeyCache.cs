using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.SigningKeys.Cache;

internal class SigningKeyCache : ISigningKeyCache
{
	private const string SigningKeysCacheKey = "RRPIdentitySigningKeys";

	private readonly IMemoryCache _cache;
	private readonly TimeSpan _cacheExpiration;

	public SigningKeyCache(IMemoryCache cache, IOptions<SigningKeysSettings> options)
	{
		_cache = cache;
		_cacheExpiration = options.Value.CachePeriod;
	}

	public bool TryGetKeys(out List<SigningKey> keys)
	{
		if (_cache.TryGetValue(SigningKeysCacheKey, out List<SigningKey> signingKeys))
		{
			keys = signingKeys;

			return true;
		}

		keys = null;

		return false;
	}

	public void CacheKeys(List<SigningKey> keys)
	{
		var cacheOptions = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheExpiration };

		_cache.Set(SigningKeysCacheKey, keys, cacheOptions);
	}
}
