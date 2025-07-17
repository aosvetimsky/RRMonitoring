using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Models;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Application.Features.PermissionGrants.Search;

public sealed class SearchPermissionGrantsRequest
	: PagedRequest,
		IRequest<PagedList<SearchPermissionGrantsResponseItem>>
{
	public IList<Guid> SourceUserIds { get; set; }
	public IList<Guid> DestinationUserIds { get; set; }
	public DateTimePeriod? GrantDates { get; set; }
	public IList<Guid> PermissionIds { get; set; }
}

public sealed class SearchPermissionGrantsRequestHandler
	: IRequestHandler<SearchPermissionGrantsRequest,
		PagedList<SearchPermissionGrantsResponseItem>>
{
	private readonly IPermissionGrantRepository _permissionGrantRepository;
	private readonly IMapper _mapper;

	public SearchPermissionGrantsRequestHandler(
		IPermissionGrantRepository permissionGrantRepository,
		IMapper mapper)
	{
		_permissionGrantRepository = permissionGrantRepository;
		_mapper = mapper;
	}

	public async Task<PagedList<SearchPermissionGrantsResponseItem>> Handle(
		SearchPermissionGrantsRequest request, CancellationToken cancellationToken)
	{
		var searchPagedCriteria = _mapper.Map<SearchPermissionGrantsPagedCriteria>(request);

		var includes = new[]
		{
			$"{nameof(PermissionGrant.SourceUser)}", $"{nameof(PermissionGrant.DestinationUser)}",
			$"{nameof(PermissionGrant.GrantedPermissions)}"
			+ $".{nameof(PermissionGrantPermission.Permission)}"
		};

		var permissionGrants = await _permissionGrantRepository.SearchWithPaging(searchPagedCriteria, includes);

		return _mapper.Map<PagedList<SearchPermissionGrantsResponseItem>>(permissionGrants);
	}
}
