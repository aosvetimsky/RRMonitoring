using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Models;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Application.Features.Roles.Search;

public class SearchRolesRequest : PagedRequest, IRequest<PagedList<SearchRolesResponseItem>>
{
	public string Keyword { get; set; }

	[SuppressMessage("Performance", "CA1819:Properties should not return arrays")] // TODO: Fix and break backward compatibility
	public Guid[] TenantIds { get; set; }
}

public class SearchRolesHandler : IRequestHandler<SearchRolesRequest, PagedList<SearchRolesResponseItem>>
{
	private readonly IRoleRepository _roleRepository;
	private readonly IMapper _mapper;

	public SearchRolesHandler(IRoleRepository roleRepository, IMapper mapper)
	{
		_roleRepository = roleRepository;
		_mapper = mapper;
	}

	public async Task<PagedList<SearchRolesResponseItem>> Handle(SearchRolesRequest request, CancellationToken cancellationToken)
	{
		var searchCriteria = _mapper.Map<SearchRolesCriteria>(request);
		var roles = await _roleRepository.SearchRoles(searchCriteria);

		return _mapper.Map<PagedList<SearchRolesResponseItem>>(roles);
	}
}
