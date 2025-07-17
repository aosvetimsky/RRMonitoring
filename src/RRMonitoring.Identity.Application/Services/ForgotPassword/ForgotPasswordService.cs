using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Features.ForgotPassword.Notification;
using RRMonitoring.Identity.Application.Features.ForgotPassword.SendCode;
using RRMonitoring.Identity.Application.Features.ForgotPassword.Verify;
using RRMonitoring.Identity.Application.Services.ForgotPassword.Models;
using RRMonitoring.Identity.Application.Services.NotificationHistory;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Application.Services.YandexSmartCaptcha;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.ForgotPassword;

internal sealed class ForgotPasswordService : IForgotPasswordService
{
	private readonly IdentityUserManager _userManager;
	private readonly IIdentityNotificationHistoryService _notificationHistoryService;
	private readonly IMediator _mediator;
	private readonly ILogger<ForgotPasswordService> _logger;
	private readonly YandexSmartCaptchaService _yandexSmartCaptchaService;

	private readonly ResetPasswordConfig _resetPasswordConfig;
	private readonly int _sendCodeTimeout;

	public ForgotPasswordService(
		IdentityUserManager userManager,
		YandexSmartCaptchaService yandexSmartCaptchaService,
		IIdentityNotificationHistoryService notificationHistoryService,
		IMediator mediator,
		ILogger<ForgotPasswordService> logger,
		IOptions<ResetPasswordConfig> resetPasswordOptions,
		IOptions<TimeoutConfig> timeoutOptions)
	{
		_yandexSmartCaptchaService = yandexSmartCaptchaService;
		_userManager = userManager;
		_notificationHistoryService = notificationHistoryService;
		_mediator = mediator;
		_logger = logger;

		_resetPasswordConfig = resetPasswordOptions.Value;
		_sendCodeTimeout = timeoutOptions.Value.SendResetPasswordCodeTimeout;
	}

	public async Task<int> GetForgotPasswordCodeTimeout(string login, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(login))
		{
			throw new ValidationException("Login is required");
		}

		var user = await GetUser(login);

		return user is not null
			? await _notificationHistoryService.GetTimeout<ResetPasswordNotification>(user.Id, _sendCodeTimeout)
			: _sendCodeTimeout;
	}

	public async Task<ForgotPasswordResponse> ForgotPasswordDirect(
		ForgotPasswordRequest request, CancellationToken cancellationToken)
	{
		try
		{
			if (_yandexSmartCaptchaService.EnabledOnForgotPassword)
			{
				var validationResultCode = await _yandexSmartCaptchaService
					.ValidateSmartToken(request.SmartToken, cancellationToken);

				if (validationResultCode != YandexSmartCaptchaValidationResultCode.Success)
				{
					var errorMessage = _yandexSmartCaptchaService.TryGetErrorMessage(validationResultCode);

					throw new ValidationException(errorMessage);
				}
			}

			var user = await GetUser(request.Login);
			if (user is not null)
			{
				await SendCode(user, request.IsViaEmail);
			}

			return new ForgotPasswordResponse { UntilResend = _sendCodeTimeout };
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Произошла ошибка при попытке восстановить пароль");

			throw;
		}
	}

	public async Task<ForgotPasswordVerifyCodeResponse> VerifyCode(
		ForgotPasswordVerifyCodeRequest request,
		CancellationToken cancellationToken)
	{
		try
		{
			var user = await GetUser(request.Email);
			if (user is null)
			{
				throw new ValidationException("Wrong code was provided");
			}

			var result =
				await _mediator.Send(
					new VerifyForgotPasswordCodeRequest { Code = request.VerificationCode, UserId = user.Id },
					cancellationToken);

			return new ForgotPasswordVerifyCodeResponse
			{
				UserId = user.Id, ResetPasswordToken = result.ResetPasswordToken
			};
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Произошла ошибка при проверке кода восстановления.");

			throw;
		}
	}

	public async Task<int> ResendCode(ForgotPasswordResendCodeRequest request, CancellationToken cancellationToken)
	{
		try
		{
			var user = await GetUser(request.Login);
			if (user is not null)
			{
				await SendCode(user, request.IsViaEmail);
			}

			return _sendCodeTimeout;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Произошла ошибка при повторной отправке кода подтверждения");

			throw;
		}
	}

	private async Task<User> GetUser(string login)
	{
		if (_resetPasswordConfig.IsResetByMobileEnabled && login.StartsWith('+'))
		{
			return await _userManager.FindByPhoneNumber(login, CancellationToken.None);
		}

		if (_resetPasswordConfig.IsResetByEmailEnabled && login.Contains('@'))
		{
			if (_resetPasswordConfig.EmailFlow == ResetPasswordConfigEmailFlow.Link)
			{
				throw new NotSupportedException("Сброс пароля по ссылке не поддерживается в DirectFlow");
			}

			return await _userManager.FindByEmailAsync(login);
		}

		if (_resetPasswordConfig.IsResetByLoginEnabled)
		{
			if (_resetPasswordConfig.EmailFlow == ResetPasswordConfigEmailFlow.Link)
			{
				throw new NotSupportedException("Сброс пароля по ссылке не поддерживается в DirectFlow");
			}

			return await _userManager.FindByNameAsync(login);
		}

		return null;
	}

	private async Task SendCode(User user, bool isSendingViaEmail = false)
	{
		var request = new SendResetPasswordCodeRequest { User = user, IsSendingViaEmail = isSendingViaEmail };

		await _mediator.Send(request);
	}
}
