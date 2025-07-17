using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.MediatR;
using Nomium.Core.Models;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Models.Manufacturer;
using RRMonitoring.Equipment.PublicModels.Manufacturers;

namespace RRMonitoring.Equipment.Application.Features.Manufacturers.Search;

public class SearchManufacturersRequest : BaseRequest<SearchManufacturersRequestDto, PagedList<SearchManufacturersResponseDto>>;

public class SearchManufacturersHandler(IManufacturerRepository manufacturerRepository, IMapper mapper) :
	BaseRequestHandler<SearchManufacturersRequest, SearchManufacturersRequestDto, PagedList<SearchManufacturersResponseDto>>
{
	protected override async Task<PagedList<SearchManufacturersResponseDto>> HandleData(SearchManufacturersRequestDto requestData, CancellationToken cancellationToken)
	{
		var searchCriteria = mapper.Map<SearchManufacturersCriteria>(requestData);

		var manufacturers = await manufacturerRepository.Search(searchCriteria, cancellationToken: cancellationToken);

		return mapper.Map<PagedList<SearchManufacturersResponseDto>>(manufacturers);
	}
}