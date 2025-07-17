using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Exceptions;
using Nomium.Core.Models;
using Nomium.Core.Security.Services.CurrentUser;
using Nomium.Core.Security.Services.CurrentUser.Models;
using RRMonitoring.Identity.Application.Services.Permissions;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Application.Features.PermissionGrants.Update;

public class UpdatePermissionGrantRequest : IRequest
{
	public Guid Id { get; set; }
	public DateTimePeriod GrantDates { get; set; }
	public IList<Guid> PermissionIds { get; set; }
}

public class UpdatePermissionGrantHandler : IRequestHandler<UpdatePermissionGrantRequest>
{
	private readonly ICurrentUserService<CurrentUserBase> _currentUserService;
	private readonly IUserRepository _userRepository;
	private readonly IPermissionGrantRepository _permissionGrantRepository;
	private readonly IPermissionValidator _permissionValidator;
	private readonly IMapper _mapper;

	public UpdatePermissionGrantHandler(
		ICurrentUserService<CurrentUserBase> currentUserService,
		IUserRepository userRepository,
		IPermissionGrantRepository permissionGrantRepository,
		IPermissionValidator permissionValidator,
		IMapper mapper)
	{
		_currentUserService = currentUserService;
		_userRepository = userRepository;
		_permissionGrantRepository = permissionGrantRepository;
		_permissionValidator = permissionValidator;
		_mapper = mapper;
	}

	public async Task<Unit> Handle(UpdatePermissionGrantRequest request, CancellationToken cancellationToken)
	{
		var permissionGrant = await _permissionGrantRepository.GetById(request.Id, new[]
		{
			$"{nameof(PermissionGrant.SourceUser)}"
				+ $".{nameof(User.UserRoles)}",
			$"{nameof(PermissionGrant.GrantedPermissions)}"
		}, cancellationToken: cancellationToken);

		if (permissionGrant == null)
		{
			throw new ValidationException($"Запись по делегированию с Id:{request.Id} не найдена.");
		}

		await ValidateIncomingRequest(request, permissionGrant);

		_mapper.Map(request, permissionGrant);
		permissionGrant.UpdatedBy = _currentUserService.CurrentUserId;

		await _permissionGrantRepository.Update(permissionGrant, cancellationToken);

		return Unit.Value;
	}

	private async Task ValidateIncomingRequest(UpdatePermissionGrantRequest request, PermissionGrant permissionGrant)
	{
		var canCurrentUserGrant = await CanUserGrantPermissions(permissionGrant.SourceUserId);
		if (!canCurrentUserGrant)
		{
			throw new ValidationException("Вы не имеете права делегировать чужие разрешения.");
		}

		var arePermissionsAvailableForSourceUser = await _permissionValidator.ArePermissionsAvailableForUser(permissionGrant.SourceUser, request.PermissionIds);
		if (!arePermissionsAvailableForSourceUser)
		{
			throw new ValidationException("Указанные разрешения недоступны для пользователя, от которого они делегируются.");
		}

		var alreadyGrantedPermissionsNames = await GetAlreadyGrantedUserPermissions(request, permissionGrant.SourceUserId);
		if (alreadyGrantedPermissionsNames.Any())
		{
			throw new ValidationException($"Разрешения: {string.Join(", ", alreadyGrantedPermissionsNames)} уже делегированы на данный период.");
		}
	}

	private async Task<IList<string>> GetAlreadyGrantedUserPermissions(UpdatePermissionGrantRequest request, Guid sourceUserId)
	{
		var searchPermissionGrantsCriteria = _mapper.Map<SearchPermissionGrantsCriteria>(request);
		searchPermissionGrantsCriteria.SourceUserIds = new[] { sourceUserId };

		var permissionsGrantedOnSamePeriod = await _permissionGrantRepository.Search(searchPermissionGrantsCriteria, new[]
		{
			$"{nameof(PermissionGrant.GrantedPermissions)}"
				+ $".{nameof(PermissionGrantPermission.Permission)}"
		});

		if (permissionsGrantedOnSamePeriod.Any(x => x.Id != request.Id))
		{
			var alreadyGrantedPermissions = permissionsGrantedOnSamePeriod
				.Where(permissionGrant => permissionGrant.Id != request.Id)
				.SelectMany(permissionGrant => permissionGrant.GrantedPermissions)
				.DistinctBy(grantedPermission => grantedPermission.PermissionId);

			var alreadyGrantedPermissionsNames = alreadyGrantedPermissions
				.IntersectBy(request.PermissionIds, grantedPermission => grantedPermission.PermissionId)
				.Select(grantedPermission => grantedPermission.Permission.DisplayName)
				.ToList();

			return alreadyGrantedPermissionsNames;
		}

		return new List<string>();
	}

	private async Task<bool> CanUserGrantPermissions(Guid sourceUserId)
	{
		var currentUser = await _userRepository.GetById(_currentUserService.CurrentUserId);

		if (currentUser.IsAdmin || currentUser.Id == sourceUserId)
		{
			return true;
		}

		return false;
	}
}
