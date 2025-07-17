using System;

namespace RRMonitoring.Mining.Domain.Entities;

public class PoolCoinAddress
{
	public Guid PoolId { get; set; }
	public Pool Pool { get; set; }

	public byte CoinId { get; set; }
	public Coin Coin { get; set; }

	public string FirstAddress { get; set; }

	public string SecondAddress { get; set; }

	public string ThirdAddress { get; set; }
}
