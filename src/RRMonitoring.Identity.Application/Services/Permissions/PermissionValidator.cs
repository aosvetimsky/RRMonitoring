using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.Permissions;

public class PermissionValidator : IPermissionValidator
{
	private readonly IPermissionRepository _permissionRepository;

	public PermissionValidator(IPermissionRepository permissionRepository)
	{
		_permissionRepository = permissionRepository;
	}

	public async Task<bool> ArePermissionsAvailableForUser(User user, IList<Guid> permissionsIds)
	{
		var roleIds = user.UserRoles.Select(userRole => userRole.RoleId).ToList();

		var rolePermissions = await _permissionRepository.GetByRoleIds(roleIds);

		var rolePermissionsIds = rolePermissions.Select(permission => permission.Id);

		return permissionsIds.All(rolePermissionsIds.Contains);
	}
}
