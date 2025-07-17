using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using RRMonitoring.Identity.Application.Enums;
using RRMonitoring.Identity.Application.Exceptions;
using RRMonitoring.Identity.Application.Features.Auth.LoginOrRegisterExternal;

namespace RRMonitoring.Identity.Api.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ExternalAuthController : Controller
{
	private readonly IMediator _mediator;
	private readonly SignInManager<User> _signInManager;
	private readonly ILogger<ExternalAuthController> _logger;

	public ExternalAuthController(
		IMediator mediator,
		SignInManager<User> signInManager,
		ILogger<ExternalAuthController> logger)
	{
		_mediator = mediator;
		_signInManager = signInManager;
		_logger = logger;
	}

	[HttpPost("external-login")]
	public IActionResult ExternalLogin([FromForm] string provider, [FromForm] string returnUrl)
	{
		var redirectUri = Url.Action(nameof(ExternalLoginCallback), "ExternalAuth", new { returnUrl });
		var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUri);

		return Challenge(properties, provider);
	}

	[HttpGet("external-login-callback")]
	[SuppressMessage("Design", "CA1031:Do not catch general exception types")] // TODO: Catch specific exception
	public async Task<IActionResult> ExternalLoginCallback(string returnUrl)
	{
		var authenticateResult = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
		if (!authenticateResult.Succeeded)
		{
			_logger.LogWarning("External authentication failed");

			return LocalRedirect(GetLoginRedirectUrl(returnUrl, ExternalAuthErrorCodes.Unauthorized));
		}

		var externalUser = authenticateResult.Principal;
		if (externalUser is null)
		{
			_logger.LogWarning("No User when try to authorized with external provider");

			return LocalRedirect(GetLoginRedirectUrl(returnUrl, ExternalAuthErrorCodes.Unknown));
		}

		try
		{
			var result = await _mediator.Send(new LoginOrRegisterExternalRequest(authenticateResult));
			if (!result.IsSuccess)
			{
				return LocalRedirect(GetLoginRedirectUrl(returnUrl, result.ErrorCode));
			}

			return LocalRedirect(returnUrl);
		}
		catch (RegisterUserException ex)
		{
			_logger.LogError(ex, "Error during external user registration with message: '{Message}'", ex.Message);

			return LocalRedirect(GetLoginRedirectUrl(returnUrl, ExternalAuthErrorCodes.Unknown));
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error during login external user with message: '{Message}'", ex.Message);

			return LocalRedirect(GetLoginRedirectUrl(returnUrl, ExternalAuthErrorCodes.Unknown));
		}
	}

	private string GetLoginRedirectUrl(string returnUrl, ExternalAuthErrorCodes? errorCode = null)
	{
		return Url.Action("Login", "Auth", new { returnUrl, errorCode })!;
	}
}
