using System.Collections.Generic;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.SigningKeys.Cache;

public interface ISigningKeyCache
{
	bool TryGetKeys(out List<SigningKey> keys);

	void CacheKeys(List<SigningKey> keys);
}
