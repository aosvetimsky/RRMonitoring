using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nomium.Core.Data.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Domain.Contracts.Repositories;

public interface IPermissionRepository : IRepository<Permission, Guid>
{
	Task<IList<Permission>> GetByRoleIds(IList<Guid> roleIds);

	Task<IList<Permission>> GetByScopeNames(IList<string> scopeNames);

	Task<IList<PermissionGroup>> GetPermissionGroupList();
}
