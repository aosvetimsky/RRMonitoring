using System;
using System.Threading.Tasks;
using Nomium.Core.Data.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Domain.Contracts.Repositories;

public interface ITenantRepository : IRepository<Tenant, Guid>
{
	Task<Tenant> GetByCode(string code);

	Task<Tenant> GetFirstByUserId(Guid userId);

	Task<PagedList<Tenant>> SearchTenants(SearchTenantsCriteria searchTenantsCriteria);
}
