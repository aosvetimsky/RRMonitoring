using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using RRMonitoring.Mining.PublicModels.Coins;

namespace RRMonitoring.Mining.ApiClients;

public interface ICoinApiClient
{
	[Get("/v1/coin/get-by-ids")]
	Task<IReadOnlyList<CoinByIdResponseDto>> GetByIds([Body] IReadOnlyList<byte> ids, CancellationToken cancellationToken);

	[Get("/v1/coin")]
	Task<IReadOnlyList<CoinResponseDto>> GetAll(CancellationToken cancellationToken);
}
