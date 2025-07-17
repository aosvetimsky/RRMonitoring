using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.ResetPassword.Verify;

public class VerifyResetPasswordTokenRequest : IRequest<bool>
{
	public Guid UserId { get; set; }

	public string Token { get; set; }

	public VerifyResetPasswordTokenRequest(Guid userId, string token)
	{
		UserId = userId;
		Token = token;
	}
}

public class VerifyResetPasswordTokenHandler : IRequestHandler<VerifyResetPasswordTokenRequest, bool>
{
	private readonly IdentityUserManager _userManager;

	public VerifyResetPasswordTokenHandler(IdentityUserManager userManager)
	{
		_userManager = userManager;
	}

	public async Task<bool> Handle(VerifyResetPasswordTokenRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.UserId.ToString());
		if (user == null)
		{
			throw new ValidationException($"Пользователь с ID: '{request.UserId}' не найден.");
		}

		var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

		return await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider,
			IdentityUserManager.ResetPasswordTokenPurpose, token);
	}
}
