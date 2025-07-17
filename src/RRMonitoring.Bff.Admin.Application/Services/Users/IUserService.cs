using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Users.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Users;

public interface IUserService
{
	Task<PagedList<SearchUsersResponseItem>> Search(SearchUsersRequest request, CancellationToken cancellationToken);

	Task UpdateRole(UpdateUserRoleRequest request, CancellationToken cancellationToken);
}
