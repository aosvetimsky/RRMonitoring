using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Identity.Domain.Contracts.Repositories;

namespace RRMonitoring.Identity.Application.Features.Permissions.GetGroupedPermissions;

public class GetGroupedPermissionsRequest : IRequest<IList<PermissionGroupResponse>>
{
}

internal class GetGroupedPermissionsHandler
	: IRequestHandler<GetGroupedPermissionsRequest,
		IList<PermissionGroupResponse>>
{
	private readonly IPermissionRepository _permissionRepository;
	private readonly IMapper _mapper;

	public GetGroupedPermissionsHandler(IPermissionRepository permissionRepository, IMapper mapper)
	{
		_permissionRepository = permissionRepository;
		_mapper = mapper;
	}

	public async Task<IList<PermissionGroupResponse>> Handle(
		GetGroupedPermissionsRequest request, CancellationToken cancellationToken)
	{
		var permissionGroups = await _permissionRepository.GetPermissionGroupList();

		return _mapper.Map<IList<PermissionGroupResponse>>(permissionGroups);
	}
}
