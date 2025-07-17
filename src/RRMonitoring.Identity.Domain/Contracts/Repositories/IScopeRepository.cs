using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;

namespace RRMonitoring.Identity.Domain.Contracts.Repositories;

public interface IScopeRepository
{
	Task<IList<ApiScope>> GetByPermissionIds(IList<Guid> permissionIds);
}
