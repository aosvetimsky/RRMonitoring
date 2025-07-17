using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RRMonitoring.Identity.Domain.Contracts;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Enums;

namespace RRMonitoring.Identity.Infrastructure.Database.IdentityStores;

public class IdentityUserStore : UserStore<User, Role, IdentityContext, Guid, IdentityUserClaim<Guid>,
	UserRole, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, IdentityRoleClaim<Guid>>, IApplicationUserStore
{
	public IdentityUserStore(IdentityContext context, IdentityErrorDescriber describer = null)
		: base(context, describer)
	{
	}

	public Task<List<UsedUserPassword>> GetLastUsedPasswords(Guid userId, int count)
	{
		return Context.Set<UsedUserPassword>()
			.Where(x => x.UserId == userId)
			.OrderByDescending(x => x.CreatedDate)
			.Take(count)
			.ToListAsync();
	}

	public async Task<DateTime?> GetEventBlockEndDate(Guid userId)
	{
		return await Context.Set<UserEvent>()
			.Where(x => x.UserId == userId)
			.MaxAsync(x => x.BlockEndDate);
	}

	public override async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default)
	{
		if (!string.IsNullOrWhiteSpace(user.PasswordHash))
		{
			var usedPassword = new UsedUserPassword { User = user, PasswordHash = user.PasswordHash };

			await Context.AddAsync(usedPassword, cancellationToken);
		}

		return await base.CreateAsync(user, cancellationToken);
	}

	public async Task AddUsedPassword(Guid userId, string passwordHash, CancellationToken cancellationToken = default)
	{
		var usedPassword = new UsedUserPassword { UserId = userId, PasswordHash = passwordHash };

		await Context.AddAsync(usedPassword, cancellationToken);
		await SaveChanges(cancellationToken);
	}

	public async Task AddEvent(
		Guid userId,
		UserEventKinds eventKind,
		DateTime? blockEndDate = null,
		CancellationToken cancellationToken = default)
	{
		var userEvent = new UserEvent { UserId = userId, EventKind = eventKind, BlockEndDate = blockEndDate };

		await Context.AddAsync(userEvent, cancellationToken);
		await SaveChanges(cancellationToken);
	}
}
