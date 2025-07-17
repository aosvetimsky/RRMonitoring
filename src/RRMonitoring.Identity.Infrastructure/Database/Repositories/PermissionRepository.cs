using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.Repositories;

internal class PermissionRepository : RepositoryBase<Permission, Guid>, IPermissionRepository
{
	private readonly IdentityContext _context;

	public PermissionRepository(IdentityContext context) : base(context)
	{
		_context = context;
	}

	public async Task<IList<Permission>> GetByRoleIds(IList<Guid> roleIds)
	{
		return await _context.Set<RolePermission>()
			.Where(x => roleIds.Contains(x.RoleId))
			.Select(x => x.Permission)
			.ToListAsync();
	}

	public async Task<IList<Permission>> GetByScopeNames(IList<string> scopeNames)
	{
		return await _context.Set<ScopePermission>()
			.Where(x => scopeNames.Contains(x.Scope.Name))
			.Select(x => x.Permission)
			.ToListAsync();
	}

	public async Task<IList<PermissionGroup>> GetPermissionGroupList()
	{
		return await _context.Set<PermissionGroup>()
			.Include(x => x.Permissions)
			.ToListAsync();
	}
}
