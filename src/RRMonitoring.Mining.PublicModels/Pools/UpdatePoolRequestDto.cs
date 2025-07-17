using System;
using System.Collections.Generic;

namespace RRMonitoring.Mining.PublicModels.Pools;

public class UpdatePoolRequestDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public IReadOnlyList<UpdatePoolCoinAddressRequestDto> CoinAddresses { get; set; }
}

public class UpdatePoolCoinAddressRequestDto
{
	public byte CoinId { get; set; }

	public string FirstAddress { get; set; }

	public string SecondAddress { get; set; }

	public string ThirdAddress { get; set; }
}
