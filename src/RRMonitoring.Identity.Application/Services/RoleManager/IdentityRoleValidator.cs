using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.RoleManager;

public class IdentityRoleValidator : IRoleValidator<Role>
{
	public IdentityRoleValidator(IdentityErrorDescriber errors = null)
	{
		_describer = errors ?? new IdentityErrorDescriber();
	}

	private readonly IdentityErrorDescriber _describer;

	public virtual async Task<IdentityResult> ValidateAsync(RoleManager<Role> manager, Role role)
	{
		if (manager == null)
		{
			throw new ArgumentNullException(nameof(manager));
		}

		if (role == null)
		{
			throw new ArgumentNullException(nameof(role));
		}

		if (manager is not IdentityRoleManager identityRoleManager)
		{
			throw new ArgumentException("RoleManager<Role> must be an instance of IdentityRoleManager");
		}

		var errors = new List<IdentityError>();
		await ValidateRoleName(identityRoleManager, role, errors);

		return errors.Count > 0
			? IdentityResult.Failed(errors.ToArray())
			: IdentityResult.Success;
	}

	private async Task ValidateRoleName(
		IdentityRoleManager manager, Role role,
		ICollection<IdentityError> errors)
	{
		var roleName = await manager.GetRoleNameAsync(role);

		if (string.IsNullOrWhiteSpace(roleName))
		{
			errors.Add(_describer.InvalidRoleName(roleName));
		}
		else
		{
			var owner = await manager.FindByNameAsync(roleName, role.TenantId);

			if (owner != null)
			{
				var ownerRoleId = await manager.GetRoleIdAsync(owner);
				var roleId = await manager.GetRoleIdAsync(role);
				if (ownerRoleId != roleId)
				{
					errors.Add(_describer.DuplicateRoleName(roleName));
				}
			}
		}
	}
}
