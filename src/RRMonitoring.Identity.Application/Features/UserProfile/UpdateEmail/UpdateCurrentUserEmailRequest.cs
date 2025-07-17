using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Extensions;
using RRMonitoring.Identity.Application.Features.UserProfile.UpdateEmail.Notification;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;
using RRMonitoring.Notification.ApiClients.Service.Notification.Rabbit;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdateEmail;

public sealed class UpdateCurrentUserEmailRequest : IRequest
{
	public string NewEmail { get; set; }

	public string TwoFactorCode { get; set; }

	public string NewEmailCode { get; set; }
}

internal sealed class UpdateCurrentUserEmailHandler : IRequestHandler<UpdateCurrentUserEmailRequest>
{
	private readonly IAccountService _accountService;
	private readonly IdentityUserManager _userManager;
	private readonly ILogger<UpdateCurrentUserEmailHandler> _logger;
	private readonly IRabbitNotificationManager _notificationManager;

	public UpdateCurrentUserEmailHandler(
		IAccountService accountService,
		IdentityUserManager userManager,
		IRabbitNotificationManager httpNotificationManager,
		ILogger<UpdateCurrentUserEmailHandler> logger)
	{
		_accountService = accountService;
		_userManager = userManager;
		_notificationManager = httpNotificationManager;

		_logger = logger;
	}

	public async Task<Unit> Handle(UpdateCurrentUserEmailRequest request, CancellationToken cancellationToken)
	{
		var userId = _accountService.GetRequiredCurrentUserId();
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null)
		{
			throw new UnauthorizedAccessException();
		}

		var isValidTwoFactorCode = await _userManager.ValidateTwoFactorCode(user, request.TwoFactorCode);
		if (!isValidTwoFactorCode)
		{
			throw new ValidationException("Недействительный 2FA код");
		}

		var oldEmail = user.Email;

		var changeEmailResult = await _userManager.ChangeEmailAsync(user, request.NewEmail, request.NewEmailCode);
		if (!changeEmailResult.Succeeded)
		{
			var errorMessages = string.Join(',', changeEmailResult.Errors.Select(x => x.Description));
			_logger.LogError("Error on email updating: {ErrorMessages}", errorMessages);

			throw new ValidationException("Ошибка при обновление email");
		}

		await SendUpdateEmailNotification(user, oldEmail);

		return Unit.Value;
	}

	private async Task SendUpdateEmailNotification(User user, string oldEmail)
	{
		var notificationsToSend = new List<NotificationBase>
		{
			new UserUpdateEmailNotification(Channels.Email)
			{
				RecipientId = user.Id, Recipient = oldEmail, Username = user.UserName, NewEmail = user.Email
			}
		};

		if (user.PhoneNumber is not null && user.PhoneNumberConfirmed)
		{
			var smsNotification = new UserUpdateEmailNotification(Channels.Sms)
			{
				Recipient = user.PhoneNumber, RecipientId = user.Id, NewEmail = user.Email
			};

			notificationsToSend.Add(smsNotification);
		}

		await _notificationManager.SendMultiple(notificationsToSend);
	}
}
