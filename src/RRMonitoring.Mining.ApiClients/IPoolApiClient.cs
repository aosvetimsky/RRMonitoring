using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using Refit;
using RRMonitoring.Mining.PublicModels.Pools;

namespace RRMonitoring.Mining.ApiClients;

public interface IPoolApiClient
{
	[Get("/v1/pool/{id}")]
	Task<PoolByIdResponseDto> GetById(Guid id, CancellationToken cancellationToken);

	[Post("/v1/pool/search")]
	Task<PagedList<SearchPoolsResponseDto>> Search([Body] SearchPoolsRequestDto requestDto, CancellationToken cancellationToken);

	[Post("/v1/pool")]
	Task<Guid> Create([Body] CreatePoolRequestDto requestDto, CancellationToken cancellationToken);

	[Put("/v1/pool")]
	Task<Guid> Update([Body] UpdatePoolRequestDto requestDto, CancellationToken cancellationToken);

	[Delete("/v1/pool/{id}")]
	Task Delete(Guid id, CancellationToken cancellationToken);
}
