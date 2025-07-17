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
using RRMonitoring.Identity.Application.Extensions;
using RRMonitoring.Identity.Application.Features.UserProfile.UpdatePhone.Notification;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;
using RRMonitoring.Notification.ApiClients.Service.Notification.Rabbit;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdatePhone;

public class UpdateCurrentUserPhoneRequest : IRequest
{
	public string TwoFactorPhoneCode { get; init; }

	public string NewPhoneCode { get; init; }

	public string NewPhoneNumber { get; init; }
}

public class UpdateCurrentUserPhoneHandler : IRequestHandler<UpdateCurrentUserPhoneRequest>
{
	private readonly IAccountService _accountService;
	private readonly IdentityUserManager _userManager;
	private readonly IRabbitNotificationManager _notificationManager;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILogger<UpdateCurrentUserPhoneHandler> _logger;

	public UpdateCurrentUserPhoneHandler(
		IAccountService accountService,
		IdentityUserManager userManager,
		IRabbitNotificationManager notificationManager,
		IDateTimeProvider dateTimeProvider,
		ILogger<UpdateCurrentUserPhoneHandler> logger)
	{
		_accountService = accountService;
		_userManager = userManager;
		_notificationManager = notificationManager;
		_dateTimeProvider = dateTimeProvider;

		_logger = logger;
	}

	public async Task<Unit> Handle(UpdateCurrentUserPhoneRequest request, CancellationToken cancellationToken)
	{
		var userId = _accountService.GetRequiredCurrentUserId();

		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null)
		{
			_logger.LogError("Try to update phone number but user with ID: {UserId} was not found.", userId);

			throw new UnauthorizedAccessException();
		}

		var existingUser = await _userManager.GetByConfirmedPhoneNumber(request.NewPhoneNumber, cancellationToken);
		if (existingUser is not null)
		{
			_logger.LogWarning("User with phone: {PhoneNumber} already exists.", request.NewPhoneNumber);

			throw new ValidationException("Данный номер телефона уже используется.");
		}

		if (user.PhoneNumber is not null && user.PhoneNumberConfirmed)
		{
			var result = await _userManager.ValidateTwoFactorCode(user, request.TwoFactorPhoneCode);
			if (!result)
			{
				throw new ValidationException("Недействительный код подтверждения");
			}
		}

		var oldPhone = user.PhoneNumberConfirmed ? user.PhoneNumber : null;

		var changePhoneResult = await _userManager.ChangePhoneNumberAsync(user, request.NewPhoneNumber, request.NewPhoneCode);
		if (!changePhoneResult.Succeeded)
		{
			var errorMessages = string.Join(',', changePhoneResult.Errors.Select(x => x.Description));
			_logger.LogError("Error on phone number updating: {ErrorMessages}", errorMessages);

			throw new ValidationException("Данные не валидны, пожалуйста, повторите попытку");
		}

		await SendUserPhoneChangedNotification(user, oldPhone);

		return Unit.Value;
	}

	private async Task SendUserPhoneChangedNotification(User user, string oldPhone)
	{
		var changeDate = $"{_dateTimeProvider.GetUtcNow():yyyy-MM-dd HH:mm} (UTC)";
		var notificationsList = new List<NotificationBase>();

		var emailNotification = new UserPhoneChangedNotification(Channels.Email)
		{
			Username = user.UserName,
			Recipient = user.Email,
			RecipientId = user.Id,
			ChangeDate = changeDate,
		};
		notificationsList.Add(emailNotification);

		if (oldPhone is not null)
		{
			var smsNotification = new UserPhoneChangedNotification(Channels.Sms)
			{
				Recipient = oldPhone,
				RecipientId = user.Id,
				ChangeDate = changeDate
			};
			notificationsList.Add(smsNotification);
		}

		await _notificationManager.SendMultiple(notificationsList);
	}
}
