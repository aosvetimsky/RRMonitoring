using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Roles.GetById;

public class GetRolesByIdsRequest : IRequest<List<RoleResponse>>
{
	public GetRolesByIdsRequest(IEnumerable<Guid> ids)
	{
		Ids = ids;
	}

	public IEnumerable<Guid> Ids { get; set; }
}

public class GetRolesByIdsHandler : IRequestHandler<GetRolesByIdsRequest, List<RoleResponse>>
{
	private readonly IRoleRepository _roleRepository;
	private readonly IMapper _mapper;

	public GetRolesByIdsHandler(
		IRoleRepository roleRepository,
		IMapper mapper)
	{
		_roleRepository = roleRepository;
		_mapper = mapper;
	}

	public async Task<List<RoleResponse>> Handle(GetRolesByIdsRequest request, CancellationToken cancellationToken)
	{
		var includes = new[]
		{
			nameof(Role.RolePermissions),
			nameof(Role.Tenant)
		};

		var roles = await _roleRepository.GetByIds(request.Ids, includes, true, cancellationToken);

		return _mapper.Map<List<RoleResponse>>(roles);
	}
}
