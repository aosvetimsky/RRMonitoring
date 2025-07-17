using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using Refit;
using RRMonitoring.Equipment.PublicModels.Manufacturers;

namespace RRMonitoring.Equipment.ApiClients;

public interface IManufacturerApiClient
{
	[Get("/v1/manufacturer/{id}")]
	Task<ManufacturerByIdResponseDto> GetById(Guid id, CancellationToken cancellationToken);

	[Post("/v1/manufacturer/search")]
	Task<PagedList<SearchManufacturersResponseDto>> Search([Body] SearchManufacturersRequestDto requestDto, CancellationToken cancellationToken);

	[Post("/v1/manufacturer")]
	Task<Guid> Create([Body] CreateManufacturerRequestDto requestDto, CancellationToken cancellationToken);

	[Put("/v1/manufacturer")]
	Task<Guid> Update([Body] UpdateManufacturerRequestDto requestDto, CancellationToken cancellationToken);

	[Delete("/v1/manufacturer/{id}")]
	Task Delete(Guid id, CancellationToken cancellationToken);
}