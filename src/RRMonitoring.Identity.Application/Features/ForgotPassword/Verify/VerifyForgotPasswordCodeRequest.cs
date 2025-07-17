using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.ForgotPassword.Verify;

public class VerifyForgotPasswordCodeRequest : IRequest<VerifyForgotPasswordCodeResponse>
{
	public Guid? UserId { get; set; }

	public string Code { get; set; }
}

public class
	VerifyForgotPasswordCodeHandler : IRequestHandler<VerifyForgotPasswordCodeRequest, VerifyForgotPasswordCodeResponse>
{
	private const string UserLockedMessage = "Пользователь заблокирован. Свяжитесь с администратором";
	private const string WrongCodeMessage = "Wrong code was provided";

	private readonly IdentityUserManager _userManager;
	private readonly bool _isUserLockoutEnabled;

	public VerifyForgotPasswordCodeHandler(
		IdentityUserManager userManager,
		IOptions<AuthenticationConfig> options)
	{
		_userManager = userManager;

		_isUserLockoutEnabled = options.Value.IsUserLockoutEnabled;
	}

	public async Task<VerifyForgotPasswordCodeResponse> Handle(
		VerifyForgotPasswordCodeRequest request, CancellationToken cancellationToken)
	{
		if (!request.UserId.HasValue)
		{
			throw new ValidationException(WrongCodeMessage);
		}

		var user = await _userManager.FindByIdAsync(request.UserId.ToString());
		if (user is null)
		{
			throw new ValidationException(WrongCodeMessage);
		}

		if (IsLockoutEnabled(user) && await _userManager.IsLockedOutAsync(user))
		{
			throw new ValidationException(UserLockedMessage);
		}

		if (!await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider,
			    IdentityUserManager.ResetPasswordTokenPurpose, request.Code))
		{
			var errorMessage = WrongCodeMessage;
			if (IsLockoutEnabled(user))
			{
				await _userManager.AccessFailedAsync(user);
				if (await _userManager.IsLockedOutAsync(user))
				{
					errorMessage = UserLockedMessage;
				}
			}

			throw new ValidationException(errorMessage);
		}

		var token = await _userManager.GeneratePasswordResetTokenAsync(user);
		token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

		return new VerifyForgotPasswordCodeResponse { ResetPasswordToken = token };
	}

	private bool IsLockoutEnabled(User user)
	{
		return _isUserLockoutEnabled && user.LockoutEnabled;
	}
}
