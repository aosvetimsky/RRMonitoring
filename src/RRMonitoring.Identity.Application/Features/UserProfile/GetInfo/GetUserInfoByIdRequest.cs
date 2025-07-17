using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Extensions;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Contracts.Repositories;

namespace RRMonitoring.Identity.Application.Features.UserProfile.GetInfo;

public class GetCurrentUserInfoRequest : IRequest<CurrentUserInfoResponse>
{
}

public class GetCurrentUserInfoHandler : IRequestHandler<GetCurrentUserInfoRequest, CurrentUserInfoResponse>
{
	private readonly IdentityUserManager _userManager;
	private readonly IAccountService _accountService;
	private readonly IRoleRepository _roleRepository;
	private readonly IPermissionRepository _permissionRepository;
	private readonly IMapper _mapper;

	public GetCurrentUserInfoHandler(
		IdentityUserManager userManager,
		IAccountService accountService,
		IRoleRepository roleRepository,
		IPermissionRepository permissionRepository,
		IMapper mapper)
	{
		_userManager = userManager;
		_accountService = accountService;
		_roleRepository = roleRepository;
		_permissionRepository = permissionRepository;
		_mapper = mapper;
	}

	public async Task<CurrentUserInfoResponse> Handle(
		GetCurrentUserInfoRequest request, CancellationToken cancellationToken)
	{
		var userId = _accountService.GetRequiredCurrentUserId();

		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null)
		{
			throw new ValidationException($"Пользователь с ID: '{userId}' не найден.");
		}

		var roles = await _roleRepository.GetByUserId(user.Id);
		var permissions = await _permissionRepository.GetByRoleIds(roles.Select(x => x.Id).ToList());

		var currentUserInfoResponse = _mapper.Map<CurrentUserInfoResponse>(user);
		currentUserInfoResponse.Permissions = permissions.Select(x => x.Name).ToList();

		var lastPasswordChangedDate = await _userManager.GetLastPasswordChangedDate(user);
		if (lastPasswordChangedDate.HasValue)
		{
			currentUserInfoResponse.LastPasswordChangedDate =
				lastPasswordChangedDate.Value.ToUniversalTime().ToString("dd.MM.yyyy");
		}

		return currentUserInfoResponse;
	}
}
