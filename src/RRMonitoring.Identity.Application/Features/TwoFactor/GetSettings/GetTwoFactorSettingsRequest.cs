using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.TwoFactor.GetSettings;

public class GetTwoFactorSettingsRequest : IRequest<GetTwoFactorSettingsResponse>
{
}

public class GetTwoFactorSettingsHandler : IRequestHandler<GetTwoFactorSettingsRequest, GetTwoFactorSettingsResponse>
{
	private readonly IAccountService _accountService;
	private readonly IdentityUserManager _userManager;
	private readonly ILogger<GetTwoFactorSettingsHandler> _logger;

	public GetTwoFactorSettingsHandler(
		IAccountService accountService,
		IdentityUserManager identityUserManager,
		ILogger<GetTwoFactorSettingsHandler> logger)
	{
		_accountService = accountService;
		_userManager = identityUserManager;

		_logger = logger;
	}

	public async Task<GetTwoFactorSettingsResponse> Handle(
		GetTwoFactorSettingsRequest request, CancellationToken cancellationToken)
	{
		var userId = _accountService.GetCurrentUserId();
		if (!userId.HasValue)
		{
			throw new UnauthorizedAccessException();
		}

		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null)
		{
			_logger.LogWarning("Try to get user with ID: {UserId}. User doesn't exist", userId);

			throw new ValidationException($"Пользователь c ID: '{userId}' не найден");
		}

		return new GetTwoFactorSettingsResponse
		{
			IsAuthenticatorEnabled = user.IsAuthenticatorEnabled,
			IsPhoneNumberSetuped = user.PhoneNumber is not null && user.PhoneNumberConfirmed
		};
	}
}
