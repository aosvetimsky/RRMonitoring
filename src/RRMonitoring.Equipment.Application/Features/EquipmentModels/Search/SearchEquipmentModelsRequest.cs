using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.MediatR;
using Nomium.Core.Models;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Models.EquipmentModel;
using RRMonitoring.Equipment.PublicModels.EquipmentModels;

namespace RRMonitoring.Equipment.Application.Features.EquipmentModels.Search;

public class SearchEquipmentModelsRequest : BaseRequest<SearchEquipmentModelsRequestDto, PagedList<SearchEquipmentModelsResponseDto>>;

public class SearchEquipmentModelsHandler(IEquipmentModelRepository equipmentModelRepository, IMapper mapper) :
	BaseRequestHandler<SearchEquipmentModelsRequest, SearchEquipmentModelsRequestDto, PagedList<SearchEquipmentModelsResponseDto>>
{
	protected override async Task<PagedList<SearchEquipmentModelsResponseDto>> HandleData(SearchEquipmentModelsRequestDto requestData, CancellationToken cancellationToken)
	{
		var searchCriteria = mapper.Map<SearchEquipmentModelsCriteria>(requestData);

		var equipmentModels = await equipmentModelRepository.Search(searchCriteria, cancellationToken: cancellationToken);

		return mapper.Map<PagedList<SearchEquipmentModelsResponseDto>>(equipmentModels);
	}
}