using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using RRMonitoring.Bff.Admin.Application.Services.Coins.Models;
using RRMonitoring.Mining.ApiClients;

namespace RRMonitoring.Bff.Admin.Application.Services.Coins;

internal class CoinService(ICoinApiClient coinApiClient, IMapper mapper) : ICoinService
{
	public async Task<IReadOnlyList<CoinResponse>> GetAll(CancellationToken cancellationToken)
	{
		var coins = await coinApiClient.GetAll(cancellationToken);

		return mapper.Map<IReadOnlyList<CoinResponse>>(coins);
	}
}
