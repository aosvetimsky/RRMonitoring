using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomium.Core.Application.Services.DateTimeProvider;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Extensions;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Enums;

namespace RRMonitoring.Identity.Application.Features.UserProfile.Delete;

public class DeleteCurrentUserRequest : IRequest
{
	public string Password { get; init; }

	public string TwoFactorCode { get; init; }
}

public class DeleteCurrentUserHandler : IRequestHandler<DeleteCurrentUserRequest>
{
	private readonly IdentityUserManager _userManager;
	private readonly IAccountService _accountService;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILogger<DeleteCurrentUserHandler> _logger;

	public DeleteCurrentUserHandler(
		IdentityUserManager userManager,
		IAccountService accountService,
		IDateTimeProvider dateTimeProvider,
		ILogger<DeleteCurrentUserHandler> logger)
	{
		_userManager = userManager;
		_accountService = accountService;
		_dateTimeProvider = dateTimeProvider;
		_logger = logger;
	}

	public async Task<Unit> Handle(DeleteCurrentUserRequest request, CancellationToken cancellationToken)
	{
		var userId = _accountService.GetRequiredCurrentUserId();
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null)
		{
			throw new UnauthorizedAccessException();
		}

		if (user.IsAdmin)
		{
			throw new ValidationException("Нельзя блокировать пользователей, являющихся системными администраторами.");
		}

		var lastEventBlockEndDate = await _userManager.GetEventBlockEndDate(user);
		if (lastEventBlockEndDate is not null &&
			lastEventBlockEndDate.Value.ToUniversalTime() > _dateTimeProvider.GetUtcNow())
		{
			throw new ValidationException(
				$"Недавно вы изменили информацию о безопасности своей учетной записи, вы можете удалить свой аккаунт после: {lastEventBlockEndDate.Value:yyyy-MM-dd HH:mm} (UTC)");
		}

		var isValidTwoFactorCode = await _userManager.ValidateTwoFactorCode(user, request.TwoFactorCode);
		if (!isValidTwoFactorCode)
		{
			throw new ValidationException("Недействительный 2FA код");
		}

		var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
		if (!isValidPassword)
		{
			throw new ValidationException("Неверный пароль");
		}

		user.IsBlocked = true;
		user.BlockedDate = _dateTimeProvider.GetUtcNow();
		user.StatusId = (byte)UserStatuses.Deleted;
		user.EmailConfirmed = false;

		var result = await _userManager.UpdateAsync(user);

		if (result.Succeeded)
		{
			return Unit.Value;
		}

		var errorDescriptions = result.Errors.Select(x => x.Description);
		_logger.LogError("Errors when try to change user blocking status: {Error}",
			string.Join(", ", errorDescriptions));

		throw new ValidationException("Ошибка при удаление пользователя.");
	}
}
