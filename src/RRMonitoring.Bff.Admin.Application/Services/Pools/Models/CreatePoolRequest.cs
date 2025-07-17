using System.Collections.Generic;

namespace RRMonitoring.Bff.Admin.Application.Services.Pools.Models;

public class CreatePoolRequest
{
	public string Name { get; set; }

	public IReadOnlyList<CreatePoolCoinAddressRequest> CoinAddresses { get; set; }
}

public class CreatePoolCoinAddressRequest
{
	public byte CoinId { get; set; }

	public string FirstAddress { get; set; }

	public string SecondAddress { get; set; }

	public string ThirdAddress { get; set; }
}
