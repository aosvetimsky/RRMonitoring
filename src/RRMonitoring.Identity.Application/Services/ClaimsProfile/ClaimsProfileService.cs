using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Nomium.Core.Application.Services.DateTimeProvider;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.ClaimsProfile;

public class ClaimsProfileService : IProfileService
{
	private readonly IdentityUserManager _userManager;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IRoleRepository _roleRepository;
	private readonly IPermissionRepository _permissionRepository;
	private readonly IPermissionGrantRepository _permissionGrantRepository;
	private readonly ITenantRepository _tenantRepository;

	public ClaimsProfileService(
		IdentityUserManager userManager,
		IDateTimeProvider dateTimeProvider,
		IRoleRepository roleRepository,
		IPermissionRepository permissionRepository,
		IPermissionGrantRepository permissionGrantRepository,
		ITenantRepository tenantRepository)
	{
		_userManager = userManager;
		_dateTimeProvider = dateTimeProvider;
		_roleRepository = roleRepository;
		_permissionRepository = permissionRepository;
		_permissionGrantRepository = permissionGrantRepository;
		_tenantRepository = tenantRepository;
	}

	public async Task GetProfileDataAsync(ProfileDataRequestContext context)
	{
		var userId = context.Subject.GetSubjectId();
		var user = await _userManager.FindByIdAsync(userId);

		if (user == null)
		{
			return;
		}

		var scopeNames = context.RequestedResources.Resources.ApiScopes.Select(x => x.Name).ToList();
		var scopePermissions = await _permissionRepository.GetByScopeNames(scopeNames);

		var userPermissions = scopePermissions;

		if (!user.IsAdmin)
		{
			var roles = await _roleRepository.GetByUserId(Guid.Parse(userId));
			var rolePermissions = await _permissionRepository.GetByRoleIds(roles.Select(x => x.Id).ToList());

			var grantedToUserPermissions = await GetGrantedToUserPermissionsIds(user.Id);

			userPermissions = rolePermissions
				.Union(grantedToUserPermissions)
				.Intersect(scopePermissions)
				.ToList();
		}

		context.IssuedClaims = userPermissions
			.Select(x => new Claim("permission", x.Name))
			.ToList();

		var tenant = await _tenantRepository.GetFirstByUserId(Guid.Parse(userId));

		context.IssuedClaims.Add(new Claim("tenant_id", tenant?.Id.ToString() ?? string.Empty));
		context.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, user.Email));
	}

	public async Task IsActiveAsync(IsActiveContext context)
	{
		var userId = context.Subject.GetSubjectId();
		var user = await _userManager.FindByIdAsync(userId);

		context.IsActive = user?.IsBlocked == false;
	}

	private async Task<IList<Permission>> GetGrantedToUserPermissionsIds(Guid userId)
	{
		var grantedPermissions = await _permissionGrantRepository.GetUserActiveGrantedPermissionsByDate(userId, _dateTimeProvider.GetUtcNow());

		return grantedPermissions
			.DistinctBy(x => x.PermissionId)
			.Select(x => x.Permission)
			.ToList();
	}
}
