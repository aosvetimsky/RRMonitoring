using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Mining.Domain.Contracts.Repositories;
using RRMonitoring.Mining.PublicModels.Coins;

namespace RRMonitoring.Mining.Application.Features.Coins.Get;

public class GetAllCoinsRequest : IRequest<IReadOnlyList<CoinResponseDto>>;

public class GetAllCoinsHandler(ICoinRepository coinRepository, IMapper mapper) : IRequestHandler<GetAllCoinsRequest, IReadOnlyList<CoinResponseDto>>
{
	public async Task<IReadOnlyList<CoinResponseDto>> Handle(GetAllCoinsRequest request, CancellationToken cancellationToken)
	{
		var coins = await coinRepository.GetAll(cancellationToken: cancellationToken);

		return mapper.Map<IReadOnlyList<CoinResponseDto>>(coins);
	}
}
