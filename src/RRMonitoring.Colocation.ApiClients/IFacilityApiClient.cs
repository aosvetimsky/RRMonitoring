using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using Refit;
using RRMonitoring.Colocation.PublicModels.Facilities;

namespace RRMonitoring.Colocation.ApiClients;

public interface IFacilityApiClient
{
	[Get("/v1/facility/{id}")]
	Task<FacilityByIdResponseDto> GetById(Guid id, CancellationToken cancellationToken);

	[Post("/v1/facility/search")]
	Task<PagedList<SearchFacilitiesResponseDto>> Search([Body] SearchFacilitiesRequestDto requestDto, CancellationToken cancellationToken);

	[Post("/v1/facility")]
	Task<Guid> Create([Body] CreateFacilityRequestDto requestDto, CancellationToken cancellationToken);

	[Put("/v1/facility")]
	Task Update([Body] UpdateFacilityRequestDto requestDto, CancellationToken cancellationToken);

	[Put("/v1/facility/{id}/archive")]
	Task Archive(Guid id, CancellationToken cancellationToken);

	[Put("/v1/facility/{id}/unarchive")]
	Task Unarchive(Guid id, CancellationToken cancellationToken);

	[Delete("/v1/facility/{id}")]
	Task Delete(Guid id, CancellationToken cancellationToken);
}
