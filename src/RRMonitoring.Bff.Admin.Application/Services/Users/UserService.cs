using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.Application.Services.DateTimeProvider;
using Nomium.Core.Models;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Bff.Admin.Application.Services.Users.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Users;

public class UserService(
	IAccountService accountService,
	IDateTimeProvider dateTimeProvider,
	IMapper mapper)
	: IUserService
{
	public Task<PagedList<SearchUsersResponseItem>> Search(SearchUsersRequest request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	public Task UpdateRole(UpdateUserRoleRequest request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
