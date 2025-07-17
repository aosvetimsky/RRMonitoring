using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.EquipmentModels.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.EquipmentModels;

public interface IEquipmentModelService
{
	Task<EquipmentModelByIdResponse> GetById(Guid id, CancellationToken cancellationToken);

	Task<PagedList<SearchEquipmentModelsResponse>> Search(SearchEquipmentModelsRequest request, CancellationToken cancellationToken);

	Task<Guid> Create(CreateEquipmentModelRequest request, CancellationToken cancellationToken);

	Task<Guid> Update(UpdateEquipmentModelRequest request, CancellationToken cancellationToken);

	Task Delete(Guid id, CancellationToken cancellationToken);
}
