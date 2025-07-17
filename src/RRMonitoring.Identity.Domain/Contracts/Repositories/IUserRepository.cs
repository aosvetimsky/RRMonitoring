using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Data.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Domain.Contracts.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
	Task<IList<Guid>> GetIdsByRoleId(Guid roleId);

	Task<PagedList<User>> SearchUsers(
		SearchUsersCriteria searchUsersCriteria,
		CancellationToken cancellationToken = default);

	Task<User> GetByExternalId(
		string externalId,
		string[] includePaths = null,
		CancellationToken cancellationToken = default);

	Task<List<User>> GetByUserNames(
		IEnumerable<string> userNames,
		CancellationToken cancellationToken = default);
}
