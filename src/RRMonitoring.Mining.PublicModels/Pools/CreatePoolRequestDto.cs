using System.Collections.Generic;

namespace RRMonitoring.Mining.PublicModels.Pools;

public class CreatePoolRequestDto
{
	public string Name { get; set; }

	public IReadOnlyList<CreatePoolCoinAddressRequestDto> CoinAddresses { get; set; }
}

public class CreatePoolCoinAddressRequestDto
{
	public byte CoinId { get; set; }

	public string FirstAddress { get; set; }

	public string SecondAddress { get; set; }

	public string ThirdAddress { get; set; }
}
