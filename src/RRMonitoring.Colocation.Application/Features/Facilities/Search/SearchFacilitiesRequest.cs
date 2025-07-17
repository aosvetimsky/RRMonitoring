using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.MediatR;
using Nomium.Core.Models;
using RRMonitoring.Colocation.Domain.Contracts.Repositories;
using RRMonitoring.Colocation.Domain.Models.Facility;
using RRMonitoring.Colocation.PublicModels.Facilities;

namespace RRMonitoring.Colocation.Application.Features.Facilities.Search;

public class SearchFacilitiesRequest : BaseRequest<SearchFacilitiesRequestDto, PagedList<SearchFacilitiesResponseDto>>;

public class SearchFacilitiesHandler(IFacilityRepository facilityRepository, IMapper mapper) : BaseRequestHandler<SearchFacilitiesRequest, SearchFacilitiesRequestDto, PagedList<SearchFacilitiesResponseDto>>
{
	protected override async Task<PagedList<SearchFacilitiesResponseDto>> HandleData(SearchFacilitiesRequestDto requestData, CancellationToken cancellationToken)
	{
		var searchCriteria = mapper.Map<SearchFacilitiesCriteria>(requestData);

		var facilities = await facilityRepository.Search(searchCriteria, cancellationToken: cancellationToken);

		return mapper.Map<PagedList<SearchFacilitiesResponseDto>>(facilities);
	}
}
