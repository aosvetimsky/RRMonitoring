using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdateUsername;

public class UpdateCurrentUserUsernameRequest : IRequest
{
	public string NewUsername { get; set; }
	public string CurrentPassword { get; set; }
}

public class UpdateCurrentUserUsernameHandler : IRequestHandler<UpdateCurrentUserUsernameRequest>
{
	private readonly IAccountService _accountService;
	private readonly IdentityUserManager _userManager;
	private readonly ILogger<UpdateCurrentUserUsernameHandler> _logger;

	public UpdateCurrentUserUsernameHandler(
		IAccountService accountService,
		IdentityUserManager userManager,
		ILogger<UpdateCurrentUserUsernameHandler> logger)
	{
		_accountService = accountService;
		_userManager = userManager;
		_logger = logger;
	}

	public async Task<Unit> Handle(UpdateCurrentUserUsernameRequest request, CancellationToken cancellationToken)
	{
		var userId = _accountService.GetCurrentUserId();
		if (!userId.HasValue)
		{
			throw new UnauthorizedAccessException();
		}

		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null)
		{
			throw new ValidationException("Current user wasn't found");
		}

		var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);
		if (!isPasswordValid)
		{
			throw new ValidationException("Current password is incorrect");
		}

		var existingUserByLogin = await _userManager.FindByNameAsync(request.NewUsername);
		if (existingUserByLogin is not null)
		{
			throw new ValidationException("Username is unavailable");
		}

		var result = await _userManager.SetUserNameAsync(user, request.NewUsername);
		if (!result.Succeeded)
		{
			var errorMessages = string.Join(',', result.Errors.Select(x => x.Description));
			_logger.LogError("Error on username updating: {ErrorMessages}", errorMessages);

			throw new ValidationException("Error on username update");
		}

		return Unit.Value;
	}
}
