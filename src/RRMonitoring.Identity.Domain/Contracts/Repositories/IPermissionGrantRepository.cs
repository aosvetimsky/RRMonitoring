using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nomium.Core.Data.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Domain.Contracts.Repositories;

public interface IPermissionGrantRepository : IRepository<PermissionGrant, Guid>
{
	Task<List<PermissionGrantPermission>> GetUserActiveGrantedPermissionsByDate(Guid userId, DateTime dateTime);

	Task<List<PermissionGrant>> Search(SearchPermissionGrantsCriteria criteria, string[] includePaths = null);

	Task<PagedList<PermissionGrant>> SearchWithPaging(SearchPermissionGrantsPagedCriteria criteria, string[] includePaths = null);
}
