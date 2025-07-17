using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Validators;

public class TwoFactorGrantTypeValidator : IExtensionGrantValidator
{
	public string GrantType { get; } = "two_factor";

	private readonly IdentityUserManager _userManager;
	private readonly ILogger<TwoFactorGrantTypeValidator> _logger;

	public TwoFactorGrantTypeValidator(
		IdentityUserManager userManager,
		ILogger<TwoFactorGrantTypeValidator> logger)
	{
		_userManager = userManager;
		_logger = logger;
	}

	public async Task ValidateAsync(ExtensionGrantValidationContext context)
	{
		var code = context.Request.Raw.Get(TwoFactorConstants.MfaCode);
		var token = context.Request.Raw.Get(TwoFactorConstants.MfaToken);
		var userId = context.Request.Raw.Get(TwoFactorConstants.UserId);
		if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
		{
			context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
			return;
		}

		var user = await _userManager.FindByIdAsync(userId);
		if (user is null)
		{
			_logger.LogError("User with ID: {UserId} hasn't been found", userId);

			context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
			return;
		}

		var isTokenValid =
			await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider,
				IdentityUserManager.TwoFactorPurpose, token);
		if (!isTokenValid)
		{
			_logger.LogError("Mfa token invalid");

			context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, "mfa_token_invalid");
			return;
		}

		var isTwoFactorCodeValid =
			await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, code);
		if (!isTwoFactorCodeValid)
		{
			_logger.LogError("Mfa code invalid");

			context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, "mfa_code_invalid");
			return;
		}

		context.Result = new GrantValidationResult(user.Id.ToString(), GrantType);
	}
}
