using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.Users.SetPasswordInternal;

public record SetUserPasswordInternalRequest : IRequest
{
	public Guid Id { get; init; }

	public string Password { get; init; }
}

public class SetUserPasswordInternalHandler : IRequestHandler<SetUserPasswordInternalRequest>
{
	private readonly IdentityUserManager _userManager;
	private readonly ILogger<SetUserPasswordInternalHandler> _logger;

	public SetUserPasswordInternalHandler(
		IdentityUserManager userManager,
		ILogger<SetUserPasswordInternalHandler> logger)
	{
		_userManager = userManager;
		_logger = logger;
	}

	public async Task<Unit> Handle(SetUserPasswordInternalRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.Id.ToString());
		if (user is null)
		{
			throw new ValidationException($"Пользователь с ID: '{request.Id}' не найден.");
		}

		var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

		var result = await _userManager.ResetPasswordAsync(user, resetToken, request.Password);
		if (!result.Succeeded)
		{
			var errorDescriptions = result.Errors.Select(x => x.Description);
			var errorMessage = string.Join(", ", errorDescriptions);
			_logger.LogError("Errors when try to set password: {Error}", errorMessage);

			throw new ValidationException(errorMessage);
		}

		return Unit.Value;
	}
}
