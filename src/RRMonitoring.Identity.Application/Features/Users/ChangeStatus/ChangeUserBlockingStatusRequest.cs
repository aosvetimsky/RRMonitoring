using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomium.Core.Application.Services.DateTimeProvider;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Features.Users.ChangeStatus.Notification;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Enums;
using RRMonitoring.Notification.ApiClients.Models;
using RRMonitoring.Notification.ApiClients.Service.Notification.Rabbit;

namespace RRMonitoring.Identity.Application.Features.Users.ChangeStatus;

public class ChangeUserBlockingStatusRequest : IRequest
{
	public Guid Id { get; set; }
	public bool IsBlocked { get; set; }
	public string BlockReason { get; set; }
	public string BlockedByAdmin { get; set; }
	public bool IsRemovePasswordRequired { get; set; }

	public ChangeUserBlockingStatusRequest(
		Guid id, bool isBlocked, string blockReason = null, bool isRemovePasswordRequired = false)
	{
		Id = id;
		IsBlocked = isBlocked;
		BlockReason = blockReason;
		IsRemovePasswordRequired = isRemovePasswordRequired;
	}
}

public class ChangeUserBlockingStatusHandler : IRequestHandler<ChangeUserBlockingStatusRequest>
{
	private readonly IUserRepository _userRepository;
	private readonly IdentityUserManager _userManager;
	private readonly IAccountService _accountService;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILogger<ChangeUserBlockingStatusHandler> _logger;
	private readonly IRabbitNotificationManager _notificationManager;

	public ChangeUserBlockingStatusHandler(
		IUserRepository userRepository,
		IdentityUserManager userManager,
		IAccountService accountService,
		IDateTimeProvider dateTimeProvider,
		ILogger<ChangeUserBlockingStatusHandler> logger,
		IRabbitNotificationManager notificationManager
	)
	{
		_userRepository = userRepository;
		_userManager = userManager;
		_accountService = accountService;
		_dateTimeProvider = dateTimeProvider;
		_logger = logger;
		_notificationManager = notificationManager;
	}

	public async Task<Unit> Handle(ChangeUserBlockingStatusRequest request, CancellationToken cancellationToken)
	{
		var user = await GetRequiredUser(request.Id, cancellationToken);

		var currentUserId = _accountService.GetCurrentUserId();
		if (currentUserId == request.Id)
		{
			throw new ValidationException("Нельзя блокировать/разблокировать самого себя.");
		}

		user.IsBlocked = request.IsBlocked;
		if (request.IsBlocked)
		{
			user.BlockedDate = _dateTimeProvider.GetUtcNow();
		}

		if (request.IsRemovePasswordRequired)
		{
			user.PasswordHash = null;
		}

		user.StatusId = user.IsBlocked
			? (byte)UserStatuses.Blocked
			: (byte)UserStatuses.Active;

		user.BlockReason = request.BlockReason;
		user.BlockedBy = user.IsBlocked ? _accountService.GetCurrentUserId() : null;

		var result = await _userManager.UpdateAsync(user);

		if (!result.Succeeded)
		{
			var errorDescriptions = result.Errors.Select(x => x.Description);
			_logger.LogError("Errors when try to change user blocking status: {Error}",
				string.Join(", ", errorDescriptions));

			throw new ValidationException("Ошибка при изменении статуса блокировки пользователя.");
		}

		EmailNotification notification = user.IsBlocked
			? new UserBlockNotification
			{
				Recipient = user.Email,
				RecipientId = user.Id,
				Username = user.UserName,
				Reason = user.BlockReason,
				BlockDate = $"{_dateTimeProvider.GetUtcNow():yyyy-MM-dd HH:mm} (UTC)"
			}
			: new UserUnblockNotification { Recipient = user.Email, RecipientId = user.Id, Username = user.UserName };

		await _notificationManager.SendEmail(notification);

		return Unit.Value;
	}

	private async Task<User> GetRequiredUser(Guid id, CancellationToken cancellationToken)
	{
		var includes = new[] { $"{nameof(User.UserRoles)}" + $".{nameof(UserRole.Role)}" };
		var user = await _userRepository.GetById(id, includes, cancellationToken: cancellationToken);

		if (user is null)
		{
			throw new ValidationException($"Пользователь с ID: '{id}' не найден.");
		}

		if (user.IsAdmin)
		{
			throw new ValidationException(
				"Нельзя блокировать/разблокировать пользователей, являющихся системными администраторами.");
		}

		return user;
	}
}
