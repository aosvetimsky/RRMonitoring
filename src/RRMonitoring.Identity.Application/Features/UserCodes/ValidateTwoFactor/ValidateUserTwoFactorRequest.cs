using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.UserCodes.ValidateTwoFactor;

public class ValidateUserTwoFactorRequest : IRequest<bool>
{
	public Guid UserId { get; set; }

	public string Code { get; set; }
}

public class ValidateUserTwoFactorHandler : IRequestHandler<ValidateUserTwoFactorRequest, bool>
{
	private readonly IdentityUserManager _userManager;

	public ValidateUserTwoFactorHandler(IdentityUserManager userManager)
	{
		_userManager = userManager;
	}

	public async Task<bool> Handle(ValidateUserTwoFactorRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.UserId.ToString());
		if (user is null)
		{
			throw new ValidationException($"User with ID: {request.UserId} not found");
		}

		return await _userManager.ValidateTwoFactorCode(user, request.Code);
	}
}
