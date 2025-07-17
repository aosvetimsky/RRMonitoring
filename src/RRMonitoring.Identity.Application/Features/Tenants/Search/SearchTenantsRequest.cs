using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Models;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Application.Features.Tenants.Search;

public class SearchTenantsRequest : PagedRequest, IRequest<PagedList<SearchTenantsResponseItem>>
{
	public string Keyword { get; set; }
}

public class SearchTenantsHandler : IRequestHandler<SearchTenantsRequest, PagedList<SearchTenantsResponseItem>>
{
	private readonly ITenantRepository _tenantRepository;
	private readonly IMapper _mapper;

	public SearchTenantsHandler(
		ITenantRepository tenantRepository,
		IMapper mapper)
	{
		_tenantRepository = tenantRepository;
		_mapper = mapper;
	}

	public async Task<PagedList<SearchTenantsResponseItem>> Handle(
		SearchTenantsRequest request, CancellationToken cancellationToken)
	{
		var searchCriteria = _mapper.Map<SearchTenantsCriteria>(request);
		var tenants = await _tenantRepository.SearchTenants(searchCriteria);

		return _mapper.Map<PagedList<SearchTenantsResponseItem>>(tenants);
	}
}
