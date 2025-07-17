using System;
using System.Collections.Generic;

namespace RRMonitoring.Bff.Admin.Application.Services.Pools.Models;

public class PoolByIdResponse
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public IReadOnlyList<PoolCoinAddressResponse> CoinAddresses { get; set; }
}

public class PoolCoinAddressResponse
{
	public byte CoinId { get; set; }

	public string FirstAddress { get; set; }

	public string SecondAddress { get; set; }

	public string ThirdAddress { get; set; }
}
