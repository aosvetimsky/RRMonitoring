using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Api.Attributes;
using RRMonitoring.Identity.Api.Helpers;
using RRMonitoring.Identity.Api.ViewModels;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Features.ForgotPassword.SendCode;
using RRMonitoring.Identity.Application.Features.ForgotPassword.SendCodeTimeout;
using RRMonitoring.Identity.Application.Features.ForgotPassword.SendLink;
using RRMonitoring.Identity.Application.Features.ForgotPassword.Verify;
using RRMonitoring.Identity.Application.Services.Link;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Application.Services.YandexSmartCaptcha;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Api.Controllers;

[ApiController]
[Route("forgot-password", Name = "ForgotPassword")]
public class ForgotPasswordController : Controller
{
	private readonly IdentityUserManager _userManager;
	private readonly ILinkService _linkService;
	private readonly IMediator _mediator;
	private readonly ILogger<ForgotPasswordController> _logger;

	private readonly ResetPasswordConfig _resetPasswordConfig;
	private readonly YandexSmartCaptchaService _yandexSmartCaptchaService;

	private readonly string _loginRedirectUrl;

	public ForgotPasswordController(
		IdentityUserManager userManager,
		ILinkService linkService,
		IMediator mediator,
		ILogger<ForgotPasswordController> logger,
		IOptions<ResetPasswordConfig> options,
		YandexSmartCaptchaService yandexSmartCaptcha,
		IConfiguration configuration)
	{
		_userManager = userManager;
		_linkService = linkService;
		_mediator = mediator;
		_logger = logger;

		_resetPasswordConfig = options.Value;
		_yandexSmartCaptchaService = yandexSmartCaptcha;

		_loginRedirectUrl = configuration.GetValue<string>("DefaultRedirectUrls:LoginPage");
	}

	[HttpGet]
	public IActionResult ForgotPassword()
	{
		return View("~/Views/ForgotPassword/ForgotPassword.cshtml", MakeForgotPasswordViewModel());
	}

	[HttpPost]
	[SuppressModelStateInvalidFilter]
	[SuppressMessage("Design", "CA1031:Do not catch general exception types")] // TODO: Catch specific exception
	public async Task<IActionResult> ForgotPassword(
		[FromForm] ForgotPasswordViewModel forgotPasswordViewModel,
		CancellationToken cancellationToken)
	{
		try
		{
			if (_yandexSmartCaptchaService.EnabledOnForgotPassword)
			{
				var validationResultCode = await _yandexSmartCaptchaService.ValidateSmartToken(
					forgotPasswordViewModel.YandexCaptchaSmartToken,
					cancellationToken);
				if (validationResultCode != YandexSmartCaptchaValidationResultCode.Success)
				{
					var errorMessage = _yandexSmartCaptchaService.TryGetErrorMessage(validationResultCode);
					return View("~/Views/ForgotPassword/ForgotPassword.cshtml", MakeForgotPasswordViewModel() with
					{
						Error = errorMessage,
					});
				}
			}

			if (_resetPasswordConfig.IsResetByMobileEnabled && !forgotPasswordViewModel.IsLoginEmail)
			{
				forgotPasswordViewModel.Phone = PhoneNumberHelper.ModifyPhoneCountryCode(forgotPasswordViewModel.Phone.Trim());
				var user = await _userManager.GetByConfirmedPhoneNumber(forgotPasswordViewModel.Phone, CancellationToken.None);

				return await SendCodeAndRedirect(user);
			}

			if (_resetPasswordConfig.IsResetByEmailEnabled && forgotPasswordViewModel.IsLoginEmail)
			{
				forgotPasswordViewModel.Login = forgotPasswordViewModel.Login.Trim();
				var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Login);
				if (_resetPasswordConfig.EmailFlow == ResetPasswordConfigEmailFlow.Link)
				{
					return await SendLinkAndRedirect(user, forgotPasswordViewModel.Login);
				}

				return await SendCodeAndRedirect(user, true);
			}

			if (_resetPasswordConfig.IsResetByLoginEnabled)
			{
				var user = await _userManager.FindByNameAsync(forgotPasswordViewModel.Login);
				if (_resetPasswordConfig.EmailFlow == ResetPasswordConfigEmailFlow.Link)
				{
					return await SendLinkAndRedirect(user, forgotPasswordViewModel.Login);
				}

				return await SendCodeAndRedirect(user, true);
			}

			throw new ValidationException("Пользователь не найден или сброс пароля недоступен");
		}
		catch (ValidationException e)
		{
			return View("~/Views/ForgotPassword/ForgotPassword.cshtml", MakeForgotPasswordViewModel() with
			{
				Error = e.Message,
			});
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Произошла ошибка при попытке восстановить пароль");

			return View("~/Views/ForgotPassword/ForgotPassword.cshtml", MakeForgotPasswordViewModel() with
			{
				Error = "Возникла ошибка на сервере. Свяжитесь с поддержкой или попробуйте позже.",
			});
		}
	}

	private async Task<RedirectResult> SendCodeAndRedirect(User user, bool isSendingViaEmail = false)
	{
		if (user is not null)
		{
			var request = new SendResetPasswordCodeRequest
			{
				User = user,
				IsSendingViaEmail = isSendingViaEmail
			};
			await _mediator.Send(request);
		}

		return Redirect(CreateLink(user?.Id, isSendingViaEmail));
	}

	private async Task<IActionResult> SendLinkAndRedirect(User user, string email)
	{
		if (user is not null)
		{
			var request = new SendResetPasswordLinkRequest(user.Email);
			await _mediator.Send(request);
		}

		var viewModel = new ForgotPasswordSuccessfulViewModel
		{
			Email = email,
			LoginUrl = _loginRedirectUrl
		};

		return View("~/Views/ForgotPassword/ForgotPasswordSuccessful.cshtml", viewModel);
	}

	[HttpGet("verify-code")]
	public async Task<IActionResult> VerifyForgotPasswordCode([FromQuery] Guid userId)
	{
		var untilResend = await _mediator.Send(new GetSendResetPasswordCodeTimeoutRequest { RecipientId = userId });

		return View("~/Views/ForgotPassword/ForgotPasswordCodeVerification.cshtml",
			new ForgotPasswordViewModel { UntilResend = untilResend });
	}

	[HttpPost("verify-code")]
	[SuppressMessage("Design", "CA1031:Do not catch general exception types")] // TODO: Catch specific exception
	public async Task<IActionResult> VerifyForgotPasswordCode(
		[FromQuery] Guid? userId,
		[FromForm] string verificationCode,
		CancellationToken cancellationToken)
	{
		try
		{
			var request = new VerifyForgotPasswordCodeRequest
			{
				UserId = userId,
				Code = verificationCode
			};
			var verifyForgotPasswordCodeResponse = await _mediator.Send(request, cancellationToken);
			var resetPasswordLink = userId.HasValue
				? _linkService.GenerateResetPasswordLink(userId.Value, verifyForgotPasswordCodeResponse.ResetPasswordToken)
				: string.Empty;

			return Redirect(resetPasswordLink);
		}
		catch (ValidationException e)
		{
			var untilResend = userId.HasValue
				? await _mediator.Send(new GetSendResetPasswordCodeTimeoutRequest { RecipientId = userId.Value },
					cancellationToken)
				: 0;

			return View("~/Views/ForgotPassword/ForgotPasswordCodeVerification.cshtml",
				new ForgotPasswordViewModel { Error = e.Message, UntilResend = untilResend });
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Произошла ошибка при проверке кода восстановления.");

			return View("~/Views/ForgotPassword/ForgotPasswordCodeVerification.cshtml",
				new ForgotPasswordViewModel { Error = "Возникла ошибка на сервере. Свяжитесь с поддержкой или попробуйте позже." }
			);
		}
	}

	[HttpPost("resend-code")]
	[SuppressMessage("Design", "CA1031:Do not catch general exception types")] // TODO: Catch specific exception
	public async Task<IActionResult> ResendCode(
		[FromForm] Guid? userId,
		[FromForm] bool isViaEmail,
		CancellationToken cancellationToken)
	{
		if (!userId.HasValue)
		{
			return View("~/Views/ForgotPassword/ForgotPassword.cshtml", MakeForgotPasswordViewModel());
		}

		try
		{
			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user is null)
			{
				throw new ValidationException($"Пользователь с UserId: '{userId}' не найден.");
			}

			await _mediator.Send(new SendResetPasswordCodeRequest { User = user, IsSendingViaEmail = isViaEmail }, cancellationToken);

			return Redirect(CreateLink(user.Id, isViaEmail));
		}
		catch (ValidationException e)
		{
			var untilResend = await _mediator.Send(new GetSendResetPasswordCodeTimeoutRequest
			{
				RecipientId = userId.Value

			}, cancellationToken);

			return View("~/Views/ForgotPassword/ForgotPasswordCodeVerification.cshtml",
				new ForgotPasswordViewModel { Error = e.Message, UntilResend = untilResend });
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Произошла ошибка при повторной отправке кода подтверждения");

			return View("~/Views/ForgotPassword/ForgotPassword.cshtml", MakeForgotPasswordViewModel() with
			{
				Error = "Возникла ошибка на сервере. Свяжитесь с поддержкой или попробуйте позже.",
			});
		}
	}

	private ForgotPasswordViewModel MakeForgotPasswordViewModel()
	{
		return new ForgotPasswordViewModel()
		{
			YandexSmartCaptchaClientSecret = _yandexSmartCaptchaService.ClientSecretIfEnabledOnLogin,
		};
	}

	private static string CreateLink(Guid? userId, bool isViaEmail)
	{
		var stringBuilder = new StringBuilder($"/forgot-password/verify-code?isViaEmail={isViaEmail}");
		if (userId.HasValue)
		{
			stringBuilder.Append($"&userId={userId}");
		}

		return stringBuilder.ToString();
	}
}
