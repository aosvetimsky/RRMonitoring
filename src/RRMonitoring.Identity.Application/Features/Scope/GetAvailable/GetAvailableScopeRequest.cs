using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Security.Services.CurrentUser;
using Nomium.Core.Security.Services.CurrentUser.Models;
using RRMonitoring.Identity.Domain.Contracts.Repositories;

namespace RRMonitoring.Identity.Application.Features.Scope.GetAvailable;

public class GetAvailableScopeRequest : IRequest<IList<ScopeResponse>>
{
}

internal class GetAvailableScopeHandler : IRequestHandler<GetAvailableScopeRequest, IList<ScopeResponse>>
{
	private readonly ICurrentUserService<CurrentUserBase> _currentUserService;
	private readonly IRoleRepository _roleRepository;
	private readonly IPermissionRepository _permissionRepository;
	private readonly IScopeRepository _scopeRepository;
	private readonly IMapper _mapper;

	public GetAvailableScopeHandler(
		ICurrentUserService<CurrentUserBase> currentUserService,
		IRoleRepository roleRepository,
		IPermissionRepository permissionRepository,
		IScopeRepository scopeRepository,
		IMapper mapper)
	{
		_currentUserService = currentUserService;
		_roleRepository = roleRepository;
		_permissionRepository = permissionRepository;
		_scopeRepository = scopeRepository;
		_mapper = mapper;
	}

	public async Task<IList<ScopeResponse>> Handle(GetAvailableScopeRequest request, CancellationToken cancellationToken)
	{
		var roles = await _roleRepository.GetByUserId(_currentUserService.CurrentUserId);

		var permissions = await _permissionRepository.GetByRoleIds(roles.Select(x => x.Id).ToList());
		var scopes = await _scopeRepository.GetByPermissionIds(permissions.Select(x => x.Id).ToList());

		return _mapper.Map<IList<ScopeResponse>>(scopes);
	}
}
