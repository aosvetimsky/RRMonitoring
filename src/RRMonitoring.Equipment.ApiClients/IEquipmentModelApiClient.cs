using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using Refit;
using RRMonitoring.Equipment.PublicModels.EquipmentModels;

namespace RRMonitoring.Equipment.ApiClients;

public interface IEquipmentModelApiClient
{
	[Get("/v1/equipment-model/{id}")]
	Task<EquipmentModelByIdResponseDto> GetById(Guid id, CancellationToken cancellationToken);

	[Post("/v1/equipment-model/search")]
	Task<PagedList<SearchEquipmentModelsResponseDto>> Search([Body] SearchEquipmentModelsRequestDto requestDto, CancellationToken cancellationToken);

	[Post("/v1/equipment-model")]
	Task<Guid> Create([Body] CreateEquipmentModelRequestDto requestDto, CancellationToken cancellationToken);

	[Put("/v1/equipment-model")]
	Task<Guid> Update([Body] UpdateEquipmentModelRequestDto requestDto, CancellationToken cancellationToken);

	[Delete("/v1/equipment-model/{id}")]
	Task Delete(Guid id, CancellationToken cancellationToken);
}