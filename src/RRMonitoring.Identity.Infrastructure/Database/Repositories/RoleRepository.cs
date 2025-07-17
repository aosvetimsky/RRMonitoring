using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nomium.Core.Data.EntityFrameworkCore.Extensions;
using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Models;
using RRMonitoring.Identity.Infrastructure.Helpers;

namespace RRMonitoring.Identity.Infrastructure.Database.Repositories;

internal class RoleRepository : RepositoryBase<Role, Guid>, IRoleRepository
{
	private readonly IdentityContext _context;

	public RoleRepository(IdentityContext context) : base(context)
	{
		_context = context;
	}

	public async Task<IList<Role>> GetByUserId(Guid userId)
	{
		return await _context.Set<UserRole>()
			.Include(x => x.Role)
			.Where(x => x.UserId == userId)
			.Select(x => x.Role)
			.ToListAsync();
	}

	public async Task<List<Role>> GetByCodes(
		IEnumerable<string> codes, IEnumerable<string> includePaths = null,
		CancellationToken cancellationToken = default)
	{
		return await EntitiesDbSet
			.AsNoTracking()
			.AddIncludes<Role, Guid>(includePaths)
			.Where(x => codes.Contains(x.Code))
			.ToListAsync(cancellationToken);
	}

	public async Task<PagedList<Role>> SearchRoles(SearchRolesCriteria searchRolesCriteria)
	{
		var query = _context.Set<Role>()
			.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(searchRolesCriteria.Keyword))
		{
			var keyword = LikeExpressionHelperCopy.ToSubstringPattern(searchRolesCriteria.Keyword);
			query = query.Where(x => EF.Functions.ILike(x.Name, keyword, LikeExpressionHelperCopy.EscapeCharacter));
		}

		if (searchRolesCriteria.TenantIds != null)
		{
			query = query.Where(x => searchRolesCriteria.TenantIds.Contains(x.TenantId.Value));
		}

		return await query
			.ToSearchResult(searchRolesCriteria.SortExpressions, searchRolesCriteria.Skip, searchRolesCriteria.Take);
	}
}
