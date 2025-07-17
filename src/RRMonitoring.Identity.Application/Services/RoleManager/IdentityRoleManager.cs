using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.RoleManager;

public class IdentityRoleManager : RoleManager<Role>
{
	public IdentityRoleManager(
		IRoleStore<Role> store,
		IEnumerable<IRoleValidator<Role>> roleValidators,
		ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
		ILogger<IdentityRoleManager> logger)
		: base(store, roleValidators, keyNormalizer, errors, logger)
	{
	}

	public Task<bool> RoleExistsAsync(string roleName, Guid? tenantId)
	{
		var normalizedName = NormalizeKey(roleName);

		return Roles.AnyAsync(x =>
			x.NormalizedName == normalizedName &&
			x.TenantId == tenantId);
	}

	public Task<Role> FindByNameAsync(string roleName, Guid? tenantId)
	{
		var normalizedName = NormalizeKey(roleName);

		return Roles.FirstOrDefaultAsync(x =>
			x.NormalizedName == normalizedName &&
			x.TenantId == tenantId);
	}
}
