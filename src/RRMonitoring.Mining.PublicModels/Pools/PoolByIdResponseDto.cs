using System;
using System.Collections.Generic;

namespace RRMonitoring.Mining.PublicModels.Pools;

public class PoolByIdResponseDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public IReadOnlyList<PoolCoinAddressResponseDto> CoinAddresses { get; set; }
}

public class PoolCoinAddressResponseDto
{
	public byte CoinId { get; set; }

	public string FirstAddress { get; set; }

	public string SecondAddress { get; set; }

	public string ThirdAddress { get; set; }
}
