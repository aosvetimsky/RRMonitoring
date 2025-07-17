using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.UserCodes.Validate;

public class ValidateUserCodeRequest : IRequest<bool>
{
	public Guid UserId { get; set; }

	public string Purpose { get; set; }

	public string Code { get; set; }
}

public class ValidateUserCodeHandler : IRequestHandler<ValidateUserCodeRequest, bool>
{
	private readonly IdentityUserManager _userManager;

	public ValidateUserCodeHandler(IdentityUserManager userManager)
	{
		_userManager = userManager;
	}

	public async Task<bool> Handle(ValidateUserCodeRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.UserId.ToString());
		if (user is null)
		{
			throw new ValidationException($"User with ID: {request.UserId} not found");
		}

		return await _userManager.VerifyUserTokenAsync(
			user,
			TokenOptions.DefaultEmailProvider,
			request.Purpose,
			request.Code);
	}
}
