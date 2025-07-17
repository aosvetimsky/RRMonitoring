using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Extensions;
using RRMonitoring.Identity.Application.Services.TwoFactor;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.TwoFactor.SendSmsCode;

public record SendTwoFactorSmsCodeRequest() : IRequest<int>;

public class SendTwoFactorSmsCodeHandler : IRequestHandler<SendTwoFactorSmsCodeRequest, int>
{
	private readonly IAccountService _accountService;
	private readonly IdentityUserManager _userManager;
	private readonly ITwoFactorService _twoFactorService;
	private readonly ILogger<SendTwoFactorSmsCodeHandler> _logger;

	public SendTwoFactorSmsCodeHandler(
		IAccountService accountService,
		IdentityUserManager userManager,
		ITwoFactorService twoFactorService,
		ILogger<SendTwoFactorSmsCodeHandler> logger)
	{
		_accountService = accountService;
		_userManager = userManager;
		_twoFactorService = twoFactorService;

		_logger = logger;
	}

	public async Task<int> Handle(SendTwoFactorSmsCodeRequest request, CancellationToken cancellationToken)
	{
		var userId = _accountService.GetRequiredCurrentUserId();
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null)
		{
			_logger.LogError("Try to send 2FA code to phone but user with ID: {UserId} was not found.", user.Id);

			throw new UnauthorizedAccessException();
		}

		return await _twoFactorService.SendCode(user, isViaEmail: false, validateTimeout: true);
	}
}
