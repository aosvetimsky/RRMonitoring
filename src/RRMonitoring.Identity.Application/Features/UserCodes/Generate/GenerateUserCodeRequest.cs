using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.UserCodes.Generate;

public class GenerateUserCodeRequest : IRequest<string>
{
	public Guid UserId { get; set; }

	public string Purpose { get; set; }
}

public class GenerateUserCodeHandler : IRequestHandler<GenerateUserCodeRequest, string>
{
	private readonly IdentityUserManager _userManager;

	public GenerateUserCodeHandler(IdentityUserManager userManager)
	{
		_userManager = userManager;
	}

	public async Task<string> Handle(GenerateUserCodeRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.UserId.ToString());
		if (user is null)
		{
			throw new ValidationException($"User with ID: {request.UserId} not found");
		}

		return await _userManager.GenerateUserTokenAsync(user, TokenOptions.DefaultEmailProvider, request.Purpose);
	}
}
