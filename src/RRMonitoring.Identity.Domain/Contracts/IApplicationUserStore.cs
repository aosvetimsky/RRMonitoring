using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Enums;

namespace RRMonitoring.Identity.Domain.Contracts;

public interface IApplicationUserStore : IUserStore<User>
{
	Task<List<UsedUserPassword>> GetLastUsedPasswords(Guid userId, int count);

	Task<DateTime?> GetEventBlockEndDate(Guid userId);

	Task AddUsedPassword(Guid userId, string passwordHash, CancellationToken cancellationToken = default);

	Task AddEvent(
		Guid userId,
		UserEventKinds eventKind,
		DateTime? blockEndDate = null,
		CancellationToken cancellationToken = default);
}
