using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Extensions;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.GoogleAuth.UpdateAuthenticator;

public class UpdateUserAuthenticatorRequest : IRequest
{
	public bool IsEnable { get; init; }

	public string Code { get; init; }
}

public class UpdateUserAuthenticatorHandler : IRequestHandler<UpdateUserAuthenticatorRequest>
{
	private readonly IAccountService _accountService;
	private readonly IdentityUserManager _userManager;

	private readonly string _tokenProvider;

	public UpdateUserAuthenticatorHandler(
		IAccountService accountService,
		IdentityUserManager userManager,
		IOptions<RedRockIdentityOptions> options)
	{
		_accountService = accountService;
		_userManager = userManager;

		_tokenProvider = options.Value.Tokens.AuthenticatorTokenProvider;
	}

	public async Task<Unit> Handle(UpdateUserAuthenticatorRequest request, CancellationToken cancellationToken)
	{
		var userId = _accountService.GetRequiredCurrentUserId();
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null)
		{
			throw new UnauthorizedAccessException();
		}

		if (request.IsEnable == user.IsAuthenticatorEnabled)
		{
			throw new ValidationException($"Authenticator уже {(request.IsEnable ? "включен" : "выключен")}");
		}

		var isCodeValid = await _userManager.VerifyTwoFactorTokenAsync(user, _tokenProvider, request.Code);
		if (!isCodeValid)
		{
			throw new ValidationException("Неправильный код подтверждения");
		}

		await _userManager.ChangeAuthenticatorEnabled(user, request.IsEnable);

		return Unit.Value;
	}
}
