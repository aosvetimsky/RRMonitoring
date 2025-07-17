using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

public class UserRepository : RepositoryBase<User, Guid>, IUserRepository
{
	private readonly DbSet<User> _usersDbSet;

	public UserRepository(IdentityContext identityContext) : base(identityContext)
	{
		_usersDbSet = identityContext.Set<User>();
	}

	[SuppressMessage("Globalization", "CA1304:Specify CultureInfo")] // TODO: Use `ToUpperInvariant()`
	[SuppressMessage("Globalization",
		"CA1311:Specify a culture or use an invariant version")] // TODO: Use `ToUpperInvariant()`
	public async Task<PagedList<User>> SearchUsers(
		SearchUsersCriteria searchUsersCriteria,
		CancellationToken cancellationToken = default)
	{
		var query = _usersDbSet
			.Include(u => u.UserRoles)
			.ThenInclude(ur => ur.Role)
			.Include(u => u.Status)
			.Include(u => u.BlockedUser)
			.AsQueryable()
			.AsNoTracking();

		if (!string.IsNullOrWhiteSpace(searchUsersCriteria.Keyword))
		{
			var keyword = LikeExpressionHelperCopy.ToSubstringPattern(searchUsersCriteria.Keyword);
			query = query.Where(x =>
				searchUsersCriteria.CanSeeSensitiveData &&
				EF.Functions.ILike(x.Email, keyword, LikeExpressionHelperCopy.EscapeCharacter)
				|| searchUsersCriteria.CanSeeSensitiveData && EF.Functions.ILike(x.PhoneNumber, keyword,
					LikeExpressionHelperCopy.EscapeCharacter)
				|| EF.Functions.ILike(x.NormalizedUserName, keyword.ToUpper(),
					LikeExpressionHelperCopy.EscapeCharacter));
		}

		if (searchUsersCriteria.RoleIds is not null)
		{
			query = query.Where(u => u.UserRoles.Any(ur => searchUsersCriteria.RoleIds.Contains(ur.RoleId)));
		}

		if (searchUsersCriteria.StatusIds is not null)
		{
			query = query.Where(u => searchUsersCriteria.StatusIds.Contains(u.StatusId));
		}

		return await query
			.ToSearchResult(searchUsersCriteria.SortExpressions, searchUsersCriteria.Skip, searchUsersCriteria.Take);
	}

	public async Task<IList<Guid>> GetIdsByRoleId(Guid roleId)
	{
		return await Context.Set<UserRole>()
			.AsNoTracking()
			.Where(userRole => userRole.RoleId == roleId)
			.Select(userRole => userRole.UserId)
			.ToListAsync();
	}

	public async Task<User> GetByExternalId(
		string externalId,
		string[] includePaths = null,
		CancellationToken cancellationToken = default)
	{
		return await _usersDbSet
			.AsNoTracking()
			.AddIncludes<User, Guid>(includePaths)
			.SingleOrDefaultAsync(x => x.ExternalId == externalId, cancellationToken);
	}

	public async Task<List<User>> GetByUserNames(
		IEnumerable<string> userNames,
		CancellationToken cancellationToken = default)
	{
		var normalizedUserNames = userNames.Select(x => x.ToUpperInvariant());

		return await _usersDbSet
			.AsNoTracking()
			.Where(x => normalizedUserNames.Contains(x.NormalizedUserName))
			.ToListAsync(cancellationToken);
	}
}
