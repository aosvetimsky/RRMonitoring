using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.MediatR;
using Nomium.Core.Models;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Models.Firmware;
using RRMonitoring.Equipment.PublicModels.Firmware;

namespace RRMonitoring.Equipment.Application.Features.Firmwares.Search;

public class SearchFirmwareRequest : BaseRequest<SearchFirmwareRequestDto, PagedList<SearchFirmwareResponseDto>>;

public class SearchFirmwareHandler
	(IFirmwareRepository firmwareRepository,
	IMapper mapper)
	: BaseRequestHandler<SearchFirmwareRequest, SearchFirmwareRequestDto, PagedList<SearchFirmwareResponseDto>>
{
	protected override async Task<PagedList<SearchFirmwareResponseDto>> HandleData(SearchFirmwareRequestDto requestData, CancellationToken cancellationToken)
	{
		var searchCriteria = mapper.Map<SearchFirmwareCriteria>(requestData);

		var firmwares = await firmwareRepository.Search(searchCriteria, cancellationToken: cancellationToken);

		return mapper.Map<PagedList<SearchFirmwareResponseDto>>(firmwares);
	}
}
