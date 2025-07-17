using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.Repositories;

internal class ScopeRepository : IScopeRepository
{
	private readonly IdentityContext _context;

	public ScopeRepository(IdentityContext context)
	{
		_context = context;
	}

	public async Task<IList<ApiScope>> GetByPermissionIds(IList<Guid> permissionIds)
	{
		return await _context.Set<ScopePermission>()
			.Where(x => permissionIds.Contains(x.PermissionId))
			.Select(x => x.Scope)
			.Distinct()
			.ToListAsync();
	}
}
