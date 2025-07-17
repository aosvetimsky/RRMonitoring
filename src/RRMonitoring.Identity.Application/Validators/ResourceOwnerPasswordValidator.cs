using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Services.TwoFactor;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Validators;

public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
	private readonly SignInManager<User> _signInManager;
	private readonly IdentityUserManager _userManager;
	private readonly ITwoFactorService _twoFactorService;
	private readonly ILogger<ResourceOwnerPasswordValidator> _logger;

	private readonly bool _isUserLockoutEnabled;
	private readonly bool _isTwoFactorEnabled;

	public ResourceOwnerPasswordValidator(
		SignInManager<User> signInManager,
		IdentityUserManager userManager,
		ITwoFactorService twoFactorService,
		ILogger<ResourceOwnerPasswordValidator> logger,
		IOptions<AuthenticationConfig> options)
	{
		_signInManager = signInManager;
		_userManager = userManager;
		_twoFactorService = twoFactorService;
		_logger = logger;

		_isUserLockoutEnabled = options.Value.IsUserLockoutEnabled;
		_isTwoFactorEnabled = options.Value.IsTwoFactorAuthenticationEnabled;
	}

	public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
	{
		var user = await _userManager.GetByLogin(context.UserName);
		if (user is not null)
		{
			var lockoutOnFailure = _isUserLockoutEnabled && user.LockoutEnabled;
			var result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, lockoutOnFailure);
			if (result.Succeeded)
			{
				var sub = await _userManager.GetUserIdAsync(user);
				if (_isTwoFactorEnabled)
				{
					var mfaTimeout = await _twoFactorService.SendCode(user, true);

					var token = await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultProvider,
						IdentityUserManager.TwoFactorPurpose);

					var requiresTwoFactorResponse = new Dictionary<string, object>
					{
						{ TwoFactorConstants.MfaRequired, true },
						{ TwoFactorConstants.MfaToken, token },
						{ TwoFactorConstants.MfaTimeout, mfaTimeout },
						{ TwoFactorConstants.UserId, user.Id }
					};

					context.Result = new GrantValidationResult(
						TokenRequestErrors.UnauthorizedClient,
						"Required two-factor authentication",
						requiresTwoFactorResponse);
					return;
				}

				_logger.LogInformation("Credentials validated for username: {username}", context.UserName);

				context.Result = new GrantValidationResult(sub, "password");
				return;
			}

			if (result.IsLockedOut)
			{
				_logger.LogInformation("Authentication failed for username: {username}, reason: locked out",
					context.UserName);
			}
			else if (result.IsNotAllowed)
			{
				_logger.LogInformation("Authentication failed for username: {username}, reason: not allowed",
					context.UserName);
			}
			else
			{
				_logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials",
					context.UserName);
			}
		}
		else
		{
			_logger.LogInformation("No user found matching username: {username}", context.UserName);
		}

		context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
	}
}
