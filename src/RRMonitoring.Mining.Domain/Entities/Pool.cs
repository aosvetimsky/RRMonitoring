using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Mining.Domain.Entities;

public class Pool : AuditableEntity
{
	public string Name { get; set; }

	public string ExternalId { get; set; }

	public ICollection<PoolCoinAddress> CoinAddresses { get; set; } = new List<PoolCoinAddress>();

	public ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
