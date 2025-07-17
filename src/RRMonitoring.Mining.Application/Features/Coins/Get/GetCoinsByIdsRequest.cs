using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.MediatR;
using RRMonitoring.Mining.Domain.Contracts.Repositories;
using RRMonitoring.Mining.PublicModels.Coins;

namespace RRMonitoring.Mining.Application.Features.Coins.Get;

public class GetCoinsByIdsRequest : BaseRequest<IReadOnlyList<byte>, IReadOnlyList<CoinByIdResponseDto>>;

public class GetCoinsByIdsHandler(ICoinRepository coinRepository, IMapper mapper) : BaseRequestHandler<GetCoinsByIdsRequest, IReadOnlyList<byte>, IReadOnlyList<CoinByIdResponseDto>>
{
	protected override async Task<IReadOnlyList<CoinByIdResponseDto>> HandleData(IReadOnlyList<byte> requestData, CancellationToken cancellationToken)
	{
		var coins = await coinRepository.GetByIds(requestData, asNoTracking: true, cancellationToken: cancellationToken);

		return mapper.Map<IReadOnlyList<CoinByIdResponseDto>>(coins);
	}
}