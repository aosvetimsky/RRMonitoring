using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nomium.Core.Application.Services.DateTimeProvider;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Features.UserProfile.ChangePassword.Notification;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;
using RRMonitoring.Notification.ApiClients.Service.Notification.Rabbit;

namespace RRMonitoring.Identity.Application.Features.ResetPassword;

public class ResetPasswordRequest : IRequest<ResetPasswordResult>
{
	public Guid UserId { get; set; }

	public string Token { get; set; }

	public string NewPassword { get; set; }
}

public class ResetPasswordRequestHandler : IRequestHandler<ResetPasswordRequest, ResetPasswordResult>
{
	private readonly IdentityUserManager _userManager;
	private readonly IPersistedGrantService _persistedGrantService;
	private readonly IRabbitNotificationManager _notificationManager;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILogger<ResetPasswordRequestHandler> _logger;

	private readonly bool _isRevokeRefreshTokensEnabled;

	public ResetPasswordRequestHandler(
		IdentityUserManager userManager,
		IPersistedGrantService persistedGrantService,
		ILogger<ResetPasswordRequestHandler> logger,
		IRabbitNotificationManager notificationManager,
		IDateTimeProvider dateTimeProvider,
		IOptions<AuthenticationConfig> options)
	{
		_userManager = userManager;
		_persistedGrantService = persistedGrantService;
		_logger = logger;
		_notificationManager = notificationManager;
		_dateTimeProvider = dateTimeProvider;

		_isRevokeRefreshTokensEnabled = options.Value.RevokeRefreshTokensOnResetPassword;
	}

	public async Task<ResetPasswordResult> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.UserId.ToString());
		if (user is null)
		{
			_logger.LogError("Errors when try to reset current user password. User with ID:{UserId} not found",
				request.UserId.ToString());

			return ResetPasswordResult.Failed("При сбросе пароля произошла ошибка");
		}

		var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
		var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

		if (!result.Succeeded)
		{
			var errorDescriptions = result.Errors.Select(x => x.Description);
			var joinedErrorDescription = string.Join(", ", errorDescriptions);
			_logger.LogError("Errors when try to reset current user password. UserId: {UserId}; Errors: {Error}", request.UserId.ToString(), joinedErrorDescription);

			return ResetPasswordResult.Failed(joinedErrorDescription);
		}

		if (_isRevokeRefreshTokensEnabled)
		{
			await _persistedGrantService.RemoveAllGrantsAsync(user.Id.ToString());
		}

		await SendPasswordChangedNotification(user);

		return ResetPasswordResult.Succeed();
	}

	private async Task SendPasswordChangedNotification(User user)
	{
		var changeDate = $"{_dateTimeProvider.GetUtcNow():yyyy-MM-dd HH:mm} (UTC)";
		var notificationsToSend = new List<NotificationBase>
		{
			new UserPasswordChangedNotification(Channels.Email)
			{
				RecipientId = user.Id,
				Recipient = user.Email,
				Username = user.UserName,
				ChangeDate = changeDate
			}
		};

		if (user.PhoneNumber is not null && user.PhoneNumberConfirmed)
		{
			var smsNotification = new UserPasswordChangedNotification(Channels.Sms)
			{
				Recipient = user.PhoneNumber,
				RecipientId = user.Id,
				ChangeDate = changeDate
			};
			notificationsToSend.Add(smsNotification);
		}

		await _notificationManager.SendMultiple(notificationsToSend);
	}
}
