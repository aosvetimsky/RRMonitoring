using System;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.GoogleAuth.GetAuthenticatorKey;

public class GetUserAuthenticatorKeyRequest : IRequest<UserAuthenticatorKeyResponse>
{
}

public class GetUserAuthenticatorKeyHandler : IRequestHandler<GetUserAuthenticatorKeyRequest, UserAuthenticatorKeyResponse>
{
	private readonly IAccountService _accountService;
	private readonly IdentityUserManager _userManager;
	private readonly UrlEncoder _urlEncoder;

	private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

	public GetUserAuthenticatorKeyHandler(
		IAccountService accountService,
		IdentityUserManager userManager,
		UrlEncoder urlEncoder)
	{
		_accountService = accountService;
		_userManager = userManager;
		_urlEncoder = urlEncoder;
	}

	public async Task<UserAuthenticatorKeyResponse> Handle(GetUserAuthenticatorKeyRequest request, CancellationToken cancellationToken)
	{
		var userId = _accountService.GetCurrentUserId();
		if (!userId.HasValue)
		{
			throw new UnauthorizedAccessException();
		}

		var user = await _userManager.FindByIdAsync(userId.ToString());

		if (user.IsAuthenticatorEnabled)
		{
			throw new ValidationException("Authenticator уже настроен");
		}

		await _userManager.ResetAuthenticatorKeyAsync(user);

		var key = await _userManager.GetAuthenticatorKeyAsync(user);

		return new UserAuthenticatorKeyResponse
		{
			Key = key,
			FormattedKey = GenerateQrCode(user.Email, key)
		};
	}

	private string GenerateQrCode(string email, string unformattedKey)
	{
		return string.Format(
			AuthenticatorUriFormat,
			_urlEncoder.Encode("Red Rock Pool"),
			_urlEncoder.Encode(email),
			unformattedKey);
	}
}

