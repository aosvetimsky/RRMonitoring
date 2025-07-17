using System;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Mining.Domain.Entities;

public class Worker : AuditableEntity
{
	public string Name { get; set; }

	public string DisplayName { get; set; }

	public Guid? ClientId { get; set; }
	public Client Client { get; set; }

	public byte StatusId { get; set; }
	public WorkerStatus Status { get; set; }

	public Guid PoolId { get; set; }
	public Pool Pool { get; set; }

	public byte CoinId { get; set; }
	public Coin Coin { get; set; }

	public string ExternalId { get; set; }

	public string ObserverLink { get; set; }
}
