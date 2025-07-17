using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomium.Core.Application.Services.DateTimeProvider;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Enums;

namespace RRMonitoring.Identity.Application.Features.Users.ChangeStatusRange;

public class ChangeUsersBlockingStatusRequest : IRequest
{
	public List<Guid> UserIds { get; set; }
	public bool IsBlocked { get; set; }

	public ChangeUsersBlockingStatusRequest(List<Guid> userIds, bool isBlocked)
	{
		UserIds = userIds;
		IsBlocked = isBlocked;
	}
}

public class ChangeUsersBlockingStatusHandler : IRequestHandler<ChangeUsersBlockingStatusRequest>
{
	private readonly IUserRepository _userRepository;
	private readonly IdentityUserManager _userManager;
	private readonly IAccountService _accountService;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILogger<ChangeUsersBlockingStatusHandler> _logger;

	public ChangeUsersBlockingStatusHandler(
		IUserRepository userRepository,
		IdentityUserManager userManager,
		IAccountService accountService,
		IDateTimeProvider dateTimeProvider,
		ILogger<ChangeUsersBlockingStatusHandler> logger
	)
	{
		_userRepository = userRepository;
		_userManager = userManager;
		_accountService = accountService;
		_dateTimeProvider = dateTimeProvider;
		_logger = logger;
	}

	public async Task<Unit> Handle(ChangeUsersBlockingStatusRequest request, CancellationToken cancellationToken)
	{
		var users = await _userRepository.GetByIds(request.UserIds, new[]
		{
			$"{nameof(User.UserRoles)}" +
			$".{nameof(UserRole.Role)}"
		}, cancellationToken: cancellationToken);

		if (!users.Any())
		{
			throw new ValidationException($"Пользователи с заданными ID не найдены.");
		}

		if (users.Any(x => x.IsAdmin))
		{
			throw new ValidationException(
				"Нельзя блокировать/разблокировать пользователей, являющихся системными администраторами.");
		}

		var currentUserId = _accountService.GetCurrentUserId();
		if (request.UserIds.Any(x => x == currentUserId))
		{
			throw new ValidationException("Нельзя блокировать/разблокировать самого себя.");
		}

		foreach (var user in users)
		{
			user.IsBlocked = request.IsBlocked;

			if (request.IsBlocked)
			{
				user.BlockedDate = _dateTimeProvider.GetUtcNow();
			}

			user.StatusId = user.IsBlocked ? (byte)UserStatuses.Blocked : (byte)UserStatuses.Active;

			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded)
			{
				var errorDescriptions = result.Errors.Select(x => x.Description);
				_logger.LogError("Errors when try to change user blocking status: {Error}",
					string.Join(", ", errorDescriptions));
			}
		}

		return Unit.Value;
	}
}
