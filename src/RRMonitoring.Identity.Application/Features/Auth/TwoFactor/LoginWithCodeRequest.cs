using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Features.Auth.Login;
using RRMonitoring.Identity.Application.Services.Agreement;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Auth.TwoFactor;

public class LoginWithCodeRequest : IRequest<LoginResultDto>
{
	[Required(ErrorMessage = "Введите код")]
	public string Code { get; set; }
}

public class LoginWithCodeHandler : IRequestHandler<LoginWithCodeRequest, LoginResultDto>
{
	private const string UserNotExistsMessage = "Данного пользователя не существует";
	private const string UserLockedMessage = "Пользователь заблокирован на {0} минут";

	private readonly IdentityUserManager _userManager;
	private readonly IVerifiedLoginService _verifiedLoginService;
	private readonly SignInManager<User> _signInManager;
	private readonly ILogger<LoginWithCodeHandler> _logger;

	private readonly bool _isAgreementAcceptanceEnabled;

	public LoginWithCodeHandler(
		IdentityUserManager userManager,
		IVerifiedLoginService agreementCookieService,
		SignInManager<User> signInManager,
		IOptions<AuthenticationConfig> options,
		ILogger<LoginWithCodeHandler> logger)
	{
		_userManager = userManager;
		_verifiedLoginService = agreementCookieService;
		_signInManager = signInManager;
		_logger = logger;

		_isAgreementAcceptanceEnabled = options.Value.IsUserAgreementAcceptanceEnabled;
	}

	public async Task<LoginResultDto> Handle(LoginWithCodeRequest request, CancellationToken cancellationToken)
	{
		var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
		if (user is null)
		{
			return LoginResultDto.Failed(UserNotExistsMessage);
		}

		if (user.IsBlocked)
		{
			_logger.LogInformation("User with id: {UserId} is blocked", user.Id.ToString());

			return LoginResultDto.Failed(UserNotExistsMessage);
		}

		var signInResult = await _signInManager.TwoFactorAuthenticatorSignInAsync(request.Code, false, false);
		if (signInResult.IsLockedOut)
		{
			var lockoutTime = _userManager.GetMinutesTillLockoutEnd(user);

			return LoginResultDto.Failed(string.Format(UserLockedMessage, lockoutTime));
		}

		if (!signInResult.Succeeded || signInResult.IsNotAllowed)
		{
			return LoginResultDto.Failed("Неверный код подтверждения");
		}

		var agreementAcceptanceRequired = _isAgreementAcceptanceEnabled && user.IsAgreementAcceptanceRequired && user.AgreementAcceptedDate is null;
		if (agreementAcceptanceRequired)
		{
			await _verifiedLoginService.RememberVerifiedLoginAsync(user.Id.ToString());
		}

		return LoginResultDto.Success(isAgreementAcceptanceRequired: agreementAcceptanceRequired);
	}
}
