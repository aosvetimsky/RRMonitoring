using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Data.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Domain.Contracts.Repositories;

public interface IRoleRepository : IRepository<Role, Guid>
{
	Task<IList<Role>> GetByUserId(Guid userId);

	Task<PagedList<Role>> SearchRoles(SearchRolesCriteria searchRolesCriteria);

	Task<List<Role>> GetByCodes(
		IEnumerable<string> codes, IEnumerable<string> includePaths = null,
		CancellationToken cancellationToken = default);
}
