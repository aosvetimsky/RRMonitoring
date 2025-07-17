using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Api.Attributes;
using RRMonitoring.Identity.Api.Helpers;
using RRMonitoring.Identity.Api.ViewModels;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Constants;
using RRMonitoring.Identity.Application.Enums;
using RRMonitoring.Identity.Application.Features.Auth.Login;
using RRMonitoring.Identity.Application.Features.Auth.Logout;
using RRMonitoring.Identity.Application.Features.Auth.TwoFactor;
using RRMonitoring.Identity.Application.Features.ForgotPassword.GetLink;
using RRMonitoring.Identity.Application.Services.Agreement;
using RRMonitoring.Identity.Application.Services.ExternalProviders;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Application.Services.YandexSmartCaptcha;

namespace RRMonitoring.Identity.Api.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class AuthController : Controller
{
	private readonly IIdentityServerInteractionService _identityInteractionService;
	private readonly YandexSmartCaptchaService _yandexSmartCaptchaService;
	private readonly IMediator _mediator;
	private readonly ILogger<AuthController> _logger;
	private readonly IdentityUserManager _userManager;
	private readonly IVerifiedLoginService _verifiedLoginService;
	private readonly IExternalProviderService _externalProviderService;

	private readonly string _agreementUrl;
	private readonly string _registrationUrl;

	public AuthController(
		IIdentityServerInteractionService identityInteractionService,
		YandexSmartCaptchaService yandexSmartCaptchaService,
		IMediator mediator,
		IdentityUserManager userManager,
		IVerifiedLoginService verifiedLoginService,
		IExternalProviderService externalProviderService,
		ILogger<AuthController> logger,
		IOptions<AuthenticationConfig> options,
		IOptions<DefaultRedirectUrlsConfiguration> redirectUrlOptions)
	{
		_identityInteractionService = identityInteractionService;
		_yandexSmartCaptchaService = yandexSmartCaptchaService;
		_mediator = mediator;
		_userManager = userManager;
		_verifiedLoginService = verifiedLoginService;
		_externalProviderService = externalProviderService;
		_logger = logger;

		_agreementUrl = options.Value.AgreementUrl;
		_registrationUrl = redirectUrlOptions.Value.RegistrationPage;
	}

	[HttpGet("login")]
	public async Task<IActionResult> Login([FromQuery] string returnUrl, [FromQuery] ExternalAuthErrorCodes? errorCode = null)
	{
		var authContext = await _identityInteractionService.GetAuthorizationContextAsync(returnUrl);
		if (authContext is null)
		{
			return LocalRedirect("/identity/error");
		}

		var errors = errorCode.HasValue
			? new[] { ExternalAuthErrors.ExternalErrorMessages.GetValueOrDefault(errorCode.Value) }
			: null;

		var loginViewModel = new LoginViewModel
		{
			ReturnUrl = returnUrl,
			RegistrationUrl = _registrationUrl,
			ErrorList = errors,
			AdProviders = _externalProviderService.GetActiveDirectoryProviders(),
			YandexSmartCaptchaClientSecret = _yandexSmartCaptchaService.ClientSecretIfEnabledOnLogin,
		};

		_verifiedLoginService.ForgetVerifiedLogin();

		return View("~/Views/Login.cshtml", loginViewModel);
	}

	[HttpPost("login")]
	[SuppressModelStateInvalidFilter]
	[SuppressMessage("Design", "CA1031:Do not catch general exception types")] // TODO: Catch specific exception
	public async Task<IActionResult> Login(
		[FromForm] LoginViewModel loginViewModel,
		[FromQuery] string returnUrl,
		CancellationToken cancellationToken)
	{
		loginViewModel.ReturnUrl = returnUrl;
		loginViewModel.RegistrationUrl = _registrationUrl;
		loginViewModel.AdProviders = _externalProviderService.GetActiveDirectoryProviders();
		loginViewModel.YandexSmartCaptchaClientSecret = _yandexSmartCaptchaService.ClientSecretIfEnabledOnLogin;

		if (!ModelState.IsValid)
		{
			loginViewModel.ErrorList = ModelState.Values
				.Where(x => x.ValidationState == ModelValidationState.Invalid)
				.SelectMany(x => x.Errors)
				.Select(x => x.ErrorMessage)
				.ToList();

			return View("~/Views/Login.cshtml", loginViewModel);
		}

		var authContext = await _identityInteractionService.GetAuthorizationContextAsync(returnUrl);
		if (authContext is null)
		{
			return LocalRedirect("/identity/error");
		}

		if (_yandexSmartCaptchaService.EnabledOnLogin)
		{
			var validationResultCode = await _yandexSmartCaptchaService.ValidateSmartToken(
				loginViewModel.YandexCaptchaSmartToken,
				cancellationToken);
			if (validationResultCode != YandexSmartCaptchaValidationResultCode.Success)
			{
				var errorMessage = _yandexSmartCaptchaService.TryGetErrorMessage(validationResultCode);
				loginViewModel.ErrorList = new List<string> { errorMessage };
				return View("~/Views/Login.cshtml", loginViewModel);
			}
		}

		try
		{
			var loginRequest = new LoginRequest
			{
				Password = loginViewModel.Password,
				// TODO: Tech. Maybe add new TrimModelBinder
				Login = PhoneNumberHelper.ModifyPhoneCountryCode(loginViewModel.Login.Trim()),
				ReturnUrl = returnUrl
			};

			var result = await _mediator.Send(loginRequest, cancellationToken);

			if (!result.IsSuccess)
			{
				loginViewModel.ErrorList = new List<string> { result.ErrorMessage };

				return View("~/Views/Login.cshtml", loginViewModel);
			}

			if (result.ShouldChangePassword)
			{
				var user = await _userManager.GetByLogin(loginViewModel.Login);

				var getResetPasswordLinkRequest = new GetResetPasswordLinkRequest
				{
					UserId = user.Id,
					ErrorCode = ResetPasswordErrorCodes.ExpiredPassword
				};
				var resetPasswordLink = await _mediator.Send(getResetPasswordLinkRequest, cancellationToken);

				return Redirect(resetPasswordLink);
			}

			if (result.IsTwoFactorRequired)
			{
				var twoFactorViewModel = new TwoFactorAuthViewModel
				{
					ReturnUrl = returnUrl
				};

				return RedirectToAction("TwoFactorCode", "Auth", twoFactorViewModel);
			}

			if (result.ShouldAcceptAgreement)
			{
				loginViewModel.DisplayAgreementModal = true;
				loginViewModel.AgreementUrl = _agreementUrl;

				return View("~/Views/Login.cshtml", loginViewModel);
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Wrong returnUrl on login: {Login} when login", loginViewModel.Login);

			return LocalRedirect("/identity/error");
		}

		// TODO: can we use local redirect here? We need to improve it in task with redirectUrl
		return Redirect(returnUrl);
	}

	[HttpGet("two-factor-code")]
	public async Task<IActionResult> TwoFactorCode([FromQuery] string returnUrl)
	{
		var authContext = await _identityInteractionService.GetAuthorizationContextAsync(returnUrl);
		if (authContext == null)
		{
			return LocalRedirect("/identity/error");
		}

		var twoFactorViewModel = new TwoFactorAuthViewModel
		{
			ReturnUrl = returnUrl
		};

		return View("~/Views/TwoFactor.cshtml", twoFactorViewModel);
	}

	[HttpPost("two-factor-code")]
	[SuppressModelStateInvalidFilter]
	public async Task<IActionResult> TwoFactorCode(
		[FromForm] TwoFactorAuthViewModel twoFactorViewModel,
		[FromQuery] string returnUrl)
	{
		twoFactorViewModel.ReturnUrl = returnUrl;

		if (!ModelState.IsValid)
		{
			twoFactorViewModel.ErrorList = ModelState.Values
				.Where(x => x.ValidationState == ModelValidationState.Invalid)
				.SelectMany(x => x.Errors)
				.Select(x => x.ErrorMessage)
				.ToList();

			return View("~/Views/TwoFactor.cshtml", twoFactorViewModel);
		}

		var authContext = await _identityInteractionService.GetAuthorizationContextAsync(returnUrl);
		if (authContext == null)
		{
			return LocalRedirect("/identity/error");
		}

		var loginWithCodeRequest = new LoginWithCodeRequest
		{
			Code = twoFactorViewModel.Code
		};

		var result = await _mediator.Send(loginWithCodeRequest);
		if (!result.IsSuccess)
		{
			twoFactorViewModel.ErrorList = new List<string>
			{
				result.ErrorMessage
			};

			return View("~/Views/TwoFactor.cshtml", twoFactorViewModel);
		}

		if (result.ShouldAcceptAgreement)
		{
			twoFactorViewModel.DisplayAgreementModal = true;
			twoFactorViewModel.AgreementUrl = _agreementUrl;

			return View("~/Views/TwoFactor.cshtml", twoFactorViewModel);
		}

		return Redirect(returnUrl);
	}

	// TODO: Decide what should we show here
	[HttpGet("error")]
	public IActionResult Error()
	{
		return View("~/Views/Error.cshtml");
	}

	[HttpGet("logout")]
	public async Task<IActionResult> Logout([FromQuery] string logoutId)
	{
		var logoutRequest = new LogoutRequest(logoutId);
		var redirectUrl = await _mediator.Send(logoutRequest);

		return Redirect(redirectUrl);
	}
}
