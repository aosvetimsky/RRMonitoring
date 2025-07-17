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
using RRMonitoring.Identity.Application.Features.TwoFactor.Notification;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;
using RRMonitoring.Notification.ApiClients.Service.Notification.Rabbit;

namespace RRMonitoring.Identity.Application.Features.TwoFactor.ChangeState;

public record ChangeTwoFactorRequest(bool IsEnabled, string TwoFactorCode) : IRequest;

public class ChangeTwoFactorHandler : IRequestHandler<ChangeTwoFactorRequest>
{
	private readonly IAccountService _accountService;
	private readonly IdentityUserManager _userManager;
	private readonly IRabbitNotificationManager _notificationManager;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILogger<ChangeTwoFactorHandler> _logger;

	public ChangeTwoFactorHandler(
		IAccountService accountService,
		IdentityUserManager identityUserManager,
		IRabbitNotificationManager notificationManager,
		IDateTimeProvider dateTimeProvider,
		ILogger<ChangeTwoFactorHandler> logger)
	{
		_accountService = accountService;
		_userManager = identityUserManager;
		_notificationManager = notificationManager;
		_dateTimeProvider = dateTimeProvider;
		_logger = logger;
	}

	public async Task<Unit> Handle(ChangeTwoFactorRequest request, CancellationToken cancellationToken)
	{
		var userId = _accountService.GetCurrentUserId();
		if (!userId.HasValue)
		{
			throw new UnauthorizedAccessException();
		}

		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null)
		{
			_logger.LogWarning("Try to get user with ID: {UserId}. User doesn't exist", userId);

			throw new ValidationException($"Пользователь c ID: '{userId}' не найден");
		}

		if (request.IsEnabled == user.TwoFactorEnabled)
		{
			throw new ValidationException($"2FA уже {(request.IsEnabled ? "включена" : "выключена")}");
		}

		if (request.IsEnabled)
		{
			var validTwoFactorProviders = await _userManager.GetValidTwoFactorProvidersAsync(user);
			if (!user.IsAuthenticatorEnabled ||
			    !validTwoFactorProviders.Contains(_userManager.Options.Tokens.AuthenticatorTokenProvider))
			{
				throw new ValidationException("Не настроен Google Authenticator");
			}
		}
		else
		{
			var codeIsValid = await _userManager.VerifyTwoFactorTokenAsync(
				user,
				_userManager.Options.Tokens.AuthenticatorTokenProvider,
				request.TwoFactorCode);

			if (!codeIsValid)
			{
				throw new ValidationException("Недействительный код подтверждения");
			}
		}

		var result = await _userManager.SetTwoFactorEnabledAsync(user, request.IsEnabled);
		if (!result.Succeeded)
		{
			var errorDescriptions = result.Errors.Select(x => x.Description);
			_logger.LogError("Errors when try to update 2FA for user: {Error}", string.Join(", ", errorDescriptions));

			throw new ValidationException($"Ошибка при {(request.IsEnabled ? "активации" : "деактивации")} 2FA.");
		}

		await SendUpdateTwoFactorStateNotification(user, request.IsEnabled);

		return Unit.Value;
	}

	private async Task SendUpdateTwoFactorStateNotification(User user, bool isEnabled)
	{
		var changeDate = $"{_dateTimeProvider.GetUtcNow():yyyy-MM-dd HH:mm} (UTC)";
		var notificationsToSend = new List<NotificationBase>
		{
			new UserUpdateTwoFactorStateNotification(Channels.Email)
			{
				RecipientId = user.Id,
				Recipient = user.Email,
				ChangeDate = changeDate,
				// TODO: refactor it after localization implementation
				Action = isEnabled ? "включена" : "выключена"
			}
		};

		if (user.PhoneNumber is not null && user.PhoneNumberConfirmed)
		{
			var smsNotification = new UserUpdateTwoFactorStateNotification(Channels.Sms)
			{
				Recipient = user.PhoneNumber,
				RecipientId = user.Id,
				ChangeDate = changeDate,
				Action = isEnabled ? "включена" : "выключена"
			};

			notificationsToSend.Add(smsNotification);
		}

		await _notificationManager.SendMultiple(notificationsToSend);
	}
}
