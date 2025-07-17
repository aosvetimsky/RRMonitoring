using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Application.Services.DateTimeProvider;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Users.GetProfileById;

public sealed class GetUserProfileByIdRequest : IRequest<UserProfileByIdResponse>
{
	public Guid Id { get; }

	public GetUserProfileByIdRequest(Guid id)
	{
		Id = id;
	}
}

public sealed class GetUserProfileByIdHandler : IRequestHandler<GetUserProfileByIdRequest, UserProfileByIdResponse>
{
	private readonly IdentityUserManager _userManager;
	private readonly IPermissionRepository _permissionRepository;
	private readonly IPermissionGrantRepository _permissionGrantRepository;
	private readonly IRoleRepository _roleRepository;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IMapper _mapper;

	public GetUserProfileByIdHandler(
		IdentityUserManager userManager,
		IPermissionRepository permissionRepository,
		IPermissionGrantRepository permissionGrantRepository,
		IRoleRepository roleRepository,
		IDateTimeProvider dateTimeProvider,
		IMapper mapper
	)
	{
		_userManager = userManager;
		_permissionRepository = permissionRepository;
		_permissionGrantRepository = permissionGrantRepository;
		_roleRepository = roleRepository;
		_dateTimeProvider = dateTimeProvider;
		_mapper = mapper;
	}

	public async Task<UserProfileByIdResponse> Handle(
		GetUserProfileByIdRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.Id.ToString())
		           ?? throw new ResourceNotFoundException($"Пользователь с ID: '{request.Id}' не найден.");

		var roles = await _roleRepository.GetByUserId(user.Id);
		var rolePermissions = await _permissionRepository.GetByRoleIds(roles.Select(x => x.Id).ToList());
		var grantedToUserPermissions = await GetGrantedToUserPermissionsIds(user.Id);

		var userPermissions = rolePermissions.Union(grantedToUserPermissions);

		return _mapper.Map<UserProfileByIdResponse>(user) with
		{
			Permissions = userPermissions.Select(x => x.Name).ToArray()
		};
	}

	private async Task<IList<Permission>> GetGrantedToUserPermissionsIds(Guid userId)
	{
		var grantedPermissions =
			await _permissionGrantRepository.GetUserActiveGrantedPermissionsByDate(userId,
				_dateTimeProvider.GetUtcNow());

		return grantedPermissions
			.DistinctBy(x => x.PermissionId)
			.Select(x => x.Permission)
			.ToList();
	}
}
