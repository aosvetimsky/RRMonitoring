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

namespace RRMonitoring.Identity.Application.Features.PermissionGrants.Create;

public class CreatePermissionGrantRequest : IRequest<Guid>
{
	public Guid SourceUserId { get; set; }
	public Guid DestinationUserId { get; set; }
	public IList<Guid> PermissionIds { get; set; }
	public DateTimePeriod GrantDates { get; set; }
	public string Reason { get; set; }
}

public class CreatePermissionGrantHandler : IRequestHandler<CreatePermissionGrantRequest, Guid>
{
	private readonly IUserRepository _userRepository;
	private readonly IPermissionGrantRepository _permissionGrantRepository;
	private readonly ICurrentUserService<CurrentUserBase> _currentUserService;
	private readonly IPermissionValidator _permissionValidator;
	private readonly IMapper _mapper;

	public CreatePermissionGrantHandler(
		IUserRepository userRepository,
		IPermissionGrantRepository permissionGrantRepository,
		ICurrentUserService<CurrentUserBase> currentUserService,
		IPermissionValidator permissionValidator,
		IMapper mapper)
	{
		_userRepository = userRepository;
		_permissionGrantRepository = permissionGrantRepository;
		_currentUserService = currentUserService;
		_permissionValidator = permissionValidator;
		_mapper = mapper;
	}

	public async Task<Guid> Handle(CreatePermissionGrantRequest request, CancellationToken cancellationToken)
	{
		await ValidateIncomingRequest(request);

		var permissionGrant = _mapper.Map<PermissionGrant>(request);
		permissionGrant.CreatedBy = _currentUserService.CurrentUserId;

		await _permissionGrantRepository.Add(permissionGrant, cancellationToken);

		return permissionGrant.Id;
	}

	private async Task ValidateIncomingRequest(CreatePermissionGrantRequest request)
	{
		var users = await _userRepository.GetByIds(new[] { request.SourceUserId, request.DestinationUserId },
			new[] { nameof(User.UserRoles) });

		var sourceUser = users.FirstOrDefault(user => user.Id == request.SourceUserId);
		if (sourceUser == null)
		{
			throw new ValidationException($"Пользователь с Id:{request.SourceUserId} не найден.");
		}

		if (users.FirstOrDefault(x => x.Id == request.DestinationUserId) == null)
		{
			throw new ValidationException($"Пользователь с Id:{request.DestinationUserId} не найден.");
		}

		var canCurrentUserGrant = await CanUserGrantPermissions(sourceUser.Id);
		if (!canCurrentUserGrant)
		{
			throw new ValidationException("Вы не имеете права делегировать чужие разрешения.");
		}

		var arePermissionsAvailableForSourceUser =
			await _permissionValidator.ArePermissionsAvailableForUser(sourceUser, request.PermissionIds);
		if (!arePermissionsAvailableForSourceUser)
		{
			throw new ValidationException(
				"Указанные разрешения недоступны для пользователя, от которого они делегируются.");
		}

		var alreadyGrantedPermissionNames = await GetAlreadyGrantedUserPermissions(request, sourceUser);
		if (alreadyGrantedPermissionNames.Any())
		{
			throw new ValidationException(
				$"Разрешения: {string.Join(", ", alreadyGrantedPermissionNames)} уже делегированы на данный период.");
		}
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

	private async Task<IList<string>> GetAlreadyGrantedUserPermissions(
		CreatePermissionGrantRequest request, User sourceUser)
	{
		var searchPermissionGrantsCriteria = _mapper.Map<SearchPermissionGrantsCriteria>(request);
		searchPermissionGrantsCriteria.SourceUserIds = new[] { sourceUser.Id };

		var permissionGrantsOnSamePeriod = await _permissionGrantRepository.Search(searchPermissionGrantsCriteria, new[]
		{
			$"{nameof(PermissionGrant.GrantedPermissions)}"
			+ $".{nameof(PermissionGrantPermission.Permission)}"
		});

		if (permissionGrantsOnSamePeriod.Any())
		{
			var alreadyGrantedPermissions = permissionGrantsOnSamePeriod
				.SelectMany(permissionGrant => permissionGrant.GrantedPermissions)
				.DistinctBy(grantedPermission => grantedPermission.PermissionId);

			var alreadyGrantedPermissionNames = alreadyGrantedPermissions
				.IntersectBy(request.PermissionIds, grantedPermission => grantedPermission.PermissionId)
				.Select(grantedPermission => grantedPermission.Permission.DisplayName)
				.ToList();

			return alreadyGrantedPermissionNames;
		}

		return new List<string>();
	}
}
