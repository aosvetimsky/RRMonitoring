using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Api.Attributes;
using RRMonitoring.Identity.Api.ViewModels;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Constants;
using RRMonitoring.Identity.Application.Enums;
using RRMonitoring.Identity.Application.Features.ResetPassword;
using RRMonitoring.Identity.Application.Features.ResetPassword.Verify;

namespace RRMonitoring.Identity.Api.Controllers;

[ApiController]
[Route("reset-password")]
public class ResetPasswordController : Controller
{
	private readonly IMediator _mediator;
	private readonly string _pathBase;
	private readonly string _defaultLoginRedirectUrl;

	public ResetPasswordController(
		IMediator mediator,
		IConfiguration configuration,
		IOptions<DefaultRedirectUrlsConfiguration> options)
	{
		_mediator = mediator;

		_pathBase = configuration.GetValue<string>("PathBase");
		_defaultLoginRedirectUrl = options.Value.LoginPage;
	}

	[HttpGet("{userId:guid}")]
	public async Task<IActionResult> ResetPassword(
		[FromRoute] Guid userId,
		[FromQuery] string token,
		[FromQuery] ResetPasswordErrorCodes? errorCode = null)
	{
		if (!await _mediator.Send(new VerifyResetPasswordTokenRequest(userId, token)))
		{
			return View("~/Views/ResetPassword/ResetPasswordInvalidLink.cshtml");
		}

		var resetPasswordViewModel = new ResetPasswordViewModel();

		if (errorCode.HasValue)
		{
			resetPasswordViewModel.ErrorList = new List<string>
			{
				ResetPasswordErrors.ErrorMessages.GetValueOrDefault(errorCode.Value)
			};
		}

		return View("~/Views/ResetPassword/ResetPassword.cshtml", resetPasswordViewModel);
	}

	[HttpPost("{userId:guid}")]
	[SuppressModelStateInvalidFilter]
	public async Task<IActionResult> ResetPassword(
		[FromRoute] Guid userId,
		[FromQuery] string returnUrl,
		[FromQuery] string token,
		[FromForm] ResetPasswordViewModel resetPasswordModel)
	{
		var loginPageUrl = GetLoginPageUrl(returnUrl);

		if (!ModelState.IsValid)
		{
			var errorList = ModelState.Values
				.Where(x => x.ValidationState == ModelValidationState.Invalid)
				.SelectMany(x => x.Errors)
				.Select(x => x.ErrorMessage)
				.ToList();

			return View("~/Views/ResetPassword/ResetPassword.cshtml", new ResetPasswordViewModel(errorList, loginPageUrl));
		}

		var resetPasswordRequest = new ResetPasswordRequest
		{
			UserId = userId,
			Token = token,
			NewPassword = resetPasswordModel.NewPassword
		};

		var resetPasswordResult = await _mediator.Send(resetPasswordRequest);

		return resetPasswordResult.IsSuccess
			? View("~/Views/ResetPassword/ResetPasswordSuccessful.cshtml", new ResetPasswordViewModel(loginPageUrl))
			: View("~/Views/ResetPassword/ResetPassword.cshtml", new ResetPasswordViewModel(resetPasswordResult.ErrorMessage, loginPageUrl));
	}

	private string GetLoginPageUrl(string returnUrl)
	{
		return string.IsNullOrWhiteSpace(returnUrl)
			? _defaultLoginRedirectUrl
			: $"{_pathBase}/login?ReturnUrl={HttpUtility.UrlEncode(returnUrl)}";
	}
}
