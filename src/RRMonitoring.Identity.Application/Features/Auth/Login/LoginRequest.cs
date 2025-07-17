using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nomium.Core.Application.Services.DateTimeProvider;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Features.Auth.Login.Notification;
using RRMonitoring.Identity.Application.Services.Agreement;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Enums;
using RRMonitoring.Notification.ApiClients.Service.Notification.Rabbit;

namespace RRMonitoring.Identity.Application.Features.Auth.Login;

public class LoginRequest : IRequest<LoginResultDto>
{
	[Required]
	public string Login { get; set; }

	[Required]
	public string Password { get; set; }

	public string ReturnUrl { get; set; }
}

public class LoginHandler : IRequestHandler<LoginRequest, LoginResultDto>
{
	private const string UserLockedMessage = "Пользователь заблокирован на {0} минут";
	private const string WrongLoginOrPasswordMessage = "Неверный логин и/или пароль";

	private readonly IdentityUserManager _userManager;
	private readonly SignInManager<User> _signInManager;
	private readonly IVerifiedLoginService _verifiedLoginService;
	private readonly IRabbitNotificationManager _notificationManager;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILogger<LoginHandler> _logger;

	private readonly bool _isAgreementAcceptanceEnabled;
	private readonly bool _isUserLockoutEnabled;
	private readonly int? _daysAfterPasswordChangeRequired;

	public LoginHandler(
		IdentityUserManager userManager,
		SignInManager<User> signInManager,
		IVerifiedLoginService agreementCookieService,
		IRabbitNotificationManager notificationManager,
		IDateTimeProvider dateTimeProvider,
		IOptions<AuthenticationConfig> options,
		IOptions<RedRockIdentityOptions> redRockIdentityOptions,
		ILogger<LoginHandler> logger)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_verifiedLoginService = agreementCookieService;
		_notificationManager = notificationManager;
		_dateTimeProvider = dateTimeProvider;
		_logger = logger;

		_isAgreementAcceptanceEnabled = options.Value.IsUserAgreementAcceptanceEnabled;
		_isUserLockoutEnabled = options.Value.IsUserLockoutEnabled;
		_daysAfterPasswordChangeRequired = redRockIdentityOptions.Value.Password.DaysAfterPasswordChangeRequired;
	}

	public async Task<LoginResultDto> Handle(LoginRequest request, CancellationToken cancellationToken)
	{
		try
		{
			return await HandleInternal(request);
		}
		catch (UserLockedExceptions)
		{
			var user = await GetUserAsync(request);
			var lockoutTime = _userManager.GetMinutesTillLockoutEnd(user);

			return LoginResultDto.Failed(string.Format(UserLockedMessage, lockoutTime));
		}
		catch (WrongLoginOrPasswordException)
		{
			return LoginResultDto.Failed(WrongLoginOrPasswordMessage);
		}
		catch (UserNotApprovedException)
		{
			return LoginResultDto.Failed(WrongLoginOrPasswordMessage);
		}
		catch (PasswordExpiredException)
		{
			return LoginResultDto.Success(isChangingPasswordRequired: true);
		}
	}

	private async Task<LoginResultDto> HandleInternal(LoginRequest request)
	{
		var user = await GetUserAsync(request);

		try
		{
			await VerifyLoginPasswordPairAsync(user, request);
		}
		catch (UserRequiresTwoFactorException)
		{
			return LoginResultDto.Success(isTwoFactorRequired: true);
		}

		var agreementAcceptanceRequired = _isAgreementAcceptanceEnabled && user.IsAgreementAcceptanceRequired &&
		                                  user.AgreementAcceptedDate is null;
		if (agreementAcceptanceRequired)
		{
			await _verifiedLoginService.RememberVerifiedLoginAsync(user.Id.ToString());

			return LoginResultDto.Success(isAgreementAcceptanceRequired: true);
		}

		user.LastLoginDate = DateTime.UtcNow;

		await _userManager.UpdateAsync(user);

		await _signInManager.SignInAsync(user, false);

		var loginNotification = new UserLoginNotification
		{
			Recipient = user.Email,
			RecipientId = user.Id,
			Username = user.UserName,
			LoginDate = $"{_dateTimeProvider.GetUtcNow():yyyy-MM-dd HH:mm} (UTC)"
		};

		await _notificationManager.SendEmail(loginNotification);

		return LoginResultDto.Success();
	}

	private async Task<User> GetUserAsync(LoginRequest request)
	{
		var user = await _userManager.GetByLogin(request.Login);
		if (user is null)
		{
			_logger.LogInformation("Сan\'t find user with login: {Login}", request.Login);

			throw new WrongLoginOrPasswordException();
		}

		if (user.IsBlocked)
		{
			_logger.LogInformation("User with login: {Login} is blocked", request.Login);

			throw new UserLockedExceptions();
		}

		if (user.StatusId == (byte)UserStatuses.OnApproval)
		{
			_logger.LogInformation("User with login: {Login} is not approved", request.Login);

			throw new UserNotApprovedException();
		}

		return user;
	}

	private async Task VerifyLoginPasswordPairAsync(User user, LoginRequest request)
	{
		var lockoutOnFailure = _isUserLockoutEnabled && user.LockoutEnabled;

		var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, lockoutOnFailure);
		if (signInResult.IsLockedOut)
		{
			throw new UserLockedExceptions();
		}

		if (signInResult.RequiresTwoFactor)
		{
			throw new UserRequiresTwoFactorException();
		}

		if (!signInResult.Succeeded)
		{
			throw new WrongLoginOrPasswordException();
		}

		if (signInResult.Succeeded && _daysAfterPasswordChangeRequired.HasValue)
		{
			var lastChangedPasswordDate = await _userManager.GetLastPasswordChangedDate(user);
			var expirationDate = lastChangedPasswordDate?.AddDays(_daysAfterPasswordChangeRequired.Value);

			if (expirationDate.HasValue && expirationDate < _dateTimeProvider.GetUtcNow())
			{
				throw new PasswordExpiredException();
			}
		}
	}
}
