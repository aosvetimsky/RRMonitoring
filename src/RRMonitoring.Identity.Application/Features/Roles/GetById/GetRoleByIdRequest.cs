using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Roles.GetById;

public class GetRoleByIdRequest : IRequest<RoleResponse>
{
	public GetRoleByIdRequest(Guid id)
	{
		Id = id;
	}

	public Guid Id { get; set; }
}

public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdRequest, RoleResponse>
{
	private readonly IRoleRepository _roleRepository;
	private readonly IMapper _mapper;

	public GetRoleByIdHandler(IRoleRepository roleRepository, IMapper mapper)
	{
		_roleRepository = roleRepository;
		_mapper = mapper;
	}

	public async Task<RoleResponse> Handle(GetRoleByIdRequest request, CancellationToken cancellationToken)
	{
		var role = await _roleRepository.GetById(request.Id, new[]
		{
			nameof(Role.RolePermissions),
			nameof(Role.Tenant)
		}, cancellationToken: cancellationToken);

		if (role == null)
		{
			throw new ResourceNotFoundException("Роли с таким идентификатором не существует.");
		}

		return _mapper.Map<RoleResponse>(role);
	}
}
