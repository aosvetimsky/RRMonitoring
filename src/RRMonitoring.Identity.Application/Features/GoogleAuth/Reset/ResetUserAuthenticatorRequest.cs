using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.GoogleAuth.Reset;

public class ResetUserAuthenticatorRequest : IRequest
{
	public Guid UserId { get; set; }

	public string ResetAuthenticatorCode { get; set; }
}

public class ResetUserTwoFactorRequestHandler : IRequestHandler<ResetUserAuthenticatorRequest, Unit>
{
	private readonly IdentityUserManager _userManager;

	public ResetUserTwoFactorRequestHandler(IdentityUserManager userManager)
	{
		_userManager = userManager;
	}

	public async Task<Unit> Handle(ResetUserAuthenticatorRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.UserId.ToString());
		if (user is null)
		{
			throw new ValidationException($"User with id {request.UserId} not found");
		}

		if (!user.IsAuthenticatorEnabled)
		{
			throw new ValidationException("Authenticator is disabled");
		}

		var isCodeValid = await _userManager.VerifyUserTokenAsync(
			user,
			TokenOptions.DefaultPhoneProvider,
			IdentityUserManager.ResetAuthenticatorPurpose,
			request.ResetAuthenticatorCode);
		if (!isCodeValid)
		{
			throw new ValidationException("Invalid code provided");
		}

		await _userManager.ChangeAuthenticatorEnabled(user, false);

		return Unit.Value;
	}
}
