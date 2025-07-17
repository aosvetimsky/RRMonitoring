using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.PermissionGrants.GetById;

public sealed class GetPermissionGrantByIdRequest : IRequest<PermissionGrantResponse>
{
	public Guid Id { get; set; }

	public GetPermissionGrantByIdRequest(Guid id)
	{
		Id = id;
	}
}

public sealed class GetPermissionGrantByIdRequestHandler : IRequestHandler<GetPermissionGrantByIdRequest, PermissionGrantResponse>
{
	private readonly IPermissionGrantRepository _permissionGrantRepository;
	private readonly IMapper _mapper;

	public GetPermissionGrantByIdRequestHandler(
		IPermissionGrantRepository permissionGrantRepository,
		IMapper mapper)
	{
		_permissionGrantRepository = permissionGrantRepository;
		_mapper = mapper;
	}

	public async Task<PermissionGrantResponse> Handle(GetPermissionGrantByIdRequest request, CancellationToken cancellationToken)
	{
		var permissionGrant = await _permissionGrantRepository.GetById(request.Id, new[]
		{
			nameof(PermissionGrant.GrantedPermissions),
			nameof(PermissionGrant.SourceUser),
			nameof(PermissionGrant.DestinationUser)
		}, cancellationToken: cancellationToken);

		if (permissionGrant == null)
		{
			throw new ResourceNotFoundException($"Запись по делегированию с Id:{request.Id} не найдена.");
		}

		return _mapper.Map<PermissionGrantResponse>(permissionGrant);
	}
}
