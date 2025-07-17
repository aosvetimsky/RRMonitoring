using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.MediatR;
using Nomium.Core.Models;
using RRMonitoring.Mining.Domain.Contracts.Repositories;
using RRMonitoring.Mining.Domain.Models.Worker;
using RRMonitoring.Mining.PublicModels.Workers;

namespace RRMonitoring.Mining.Application.Features.Workers.Search;

public class SearchWorkersRequest : BaseRequest<SearchWorkersRequestDto, PagedList<SearchWorkersResponseDto>>;

public class SearchWorkersHandler(IWorkerRepository workerRepository, IMapper mapper) : BaseRequestHandler<SearchWorkersRequest, SearchWorkersRequestDto, PagedList<SearchWorkersResponseDto>>
{
	protected override async Task<PagedList<SearchWorkersResponseDto>> HandleData(SearchWorkersRequestDto requestData, CancellationToken cancellationToken)
	{
		var searchCriteria = mapper.Map<SearchWorkersCriteria>(requestData);

		var workers = await workerRepository.Search(searchCriteria, cancellationToken: cancellationToken);

		return mapper.Map<PagedList<SearchWorkersResponseDto>>(workers);
	}
}
