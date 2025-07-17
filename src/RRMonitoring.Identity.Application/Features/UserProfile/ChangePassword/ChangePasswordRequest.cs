using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomium.Core.Application.Services.DateTimeProvider;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Extensions;
using RRMonitoring.Identity.Application.Features.UserProfile.ChangePassword.Notification;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;
using RRMonitoring.Notification.ApiClients.Service.Notification.Rabbit;
using ValidationException = Nomium.Core.Exceptions.ValidationException;

namespace RRMonitoring.Identity.Application.Features.UserProfile.ChangePassword;

public record ChangePasswordRequest : IRequest
{
	public string TwoFactorCode { get; init; }

	public string OldPassword { get; init; }

	public string NewPassword { get; init; }

	public string ConfirmedNewPassword { get; init; }
}

public class ChangePasswordHandler : IRequestHandler<ChangePasswordRequest>
{
	private readonly IAccountService _accountService;
	private readonly IdentityUserManager _userManager;
	private readonly IRabbitNotificationManager _notificationManager;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILogger<ChangePasswordHandler> _logger;

	public ChangePasswordHandler(
		IAccountService accountService,
		IdentityUserManager userManager,
		IRabbitNotificationManager notificationManager,
		IDateTimeProvider dateTimeProvider,
		ILogger<ChangePasswordHandler> logger)
	{
		_accountService = accountService;
		_userManager = userManager;
		_notificationManager = notificationManager;
		_dateTimeProvider = dateTimeProvider;

		_logger = logger;
	}

	public async Task<Unit> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
	{
		var userId = _accountService.GetRequiredCurrentUserId();

		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null)
		{
			_logger.LogError("Try to change password but user with ID: {UserId} was not found.", userId);

			throw new UnauthorizedAccessException();
		}

		var isValidCode = await _userManager.ValidateTwoFactorCode(user, request.TwoFactorCode);
		if (!isValidCode)
		{
			throw new ValidationException("Недействильный код подтверждения");
		}

		var changePasswordResult =
			await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
		if (!changePasswordResult.Succeeded)
		{
			var errorMessages = string.Join(',', changePasswordResult.Errors.Select(x => x.Description));
			_logger.LogError("Error on password changing: {ErrorMessages}", errorMessages);

			throw new ValidationException("Данные не валидны, пожалуйста, повторите попытку");
		}

		await SendPasswordChangedNotification(user);

		return Unit.Value;
	}

	private async Task SendPasswordChangedNotification(User user)
	{
		var changeDate = $"{_dateTimeProvider.GetUtcNow():yyyy-MM-dd HH:mm} (UTC)";
		var notificationsToSend = new List<NotificationBase>
		{
			new UserPasswordChangedNotification(Channels.Email)
			{
				RecipientId = user.Id, Recipient = user.Email, Username = user.UserName, ChangeDate = changeDate
			}
		};

		if (user.PhoneNumber is not null && user.PhoneNumberConfirmed)
		{
			var smsNotification = new UserPasswordChangedNotification(Channels.Sms)
			{
				Recipient = user.PhoneNumber, RecipientId = user.Id, ChangeDate = changeDate
			};
			notificationsToSend.Add(smsNotification);
		}

		await _notificationManager.SendMultiple(notificationsToSend);
	}
}
