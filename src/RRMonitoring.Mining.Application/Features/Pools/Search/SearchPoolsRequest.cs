using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.MediatR;
using Nomium.Core.Models;
using RRMonitoring.Mining.Domain.Contracts.Repositories;
using RRMonitoring.Mining.Domain.Models.Pool;
using RRMonitoring.Mining.PublicModels.Pools;

namespace RRMonitoring.Mining.Application.Features.Pools.Search;

public class SearchPoolsRequest : BaseRequest<SearchPoolsRequestDto, PagedList<SearchPoolsResponseDto>>;

public class SearchPoolsHandler(IPoolRepository poolRepository, IMapper mapper) : BaseRequestHandler<SearchPoolsRequest, SearchPoolsRequestDto, PagedList<SearchPoolsResponseDto>>
{
	protected override async Task<PagedList<SearchPoolsResponseDto>> HandleData(SearchPoolsRequestDto requestData, CancellationToken cancellationToken)
	{
		var searchCriteria = mapper.Map<SearchPoolsCriteria>(requestData);

		var pools = await poolRepository.Search(searchCriteria, cancellationToken: cancellationToken);

		return new PagedList<SearchPoolsResponseDto>
		{
			TotalCount = pools.TotalCount,
			Items = pools.Items
				.Select(p => new SearchPoolsResponseDto
				{
					Id = p.Id,
					Name = p.Name,
					Coins = p.CoinAddresses
						.Select(c => new SearchPoolsCoinResponseDto
						{
							CoinId = c.CoinId,
							CoinName = c.Coin.Name
						})
						.ToList()
				})
				.ToList()
		};
	}
}
