using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Services.NotificationHistory;
using RRMonitoring.Identity.Application.Services.TwoFactor.Notification;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Service.Notification.Http;

namespace RRMonitoring.Identity.Application.Services.TwoFactor;

internal class TwoFactorService : ITwoFactorService
{
	private readonly IHttpNotificationManager _notificationManager;
	private readonly IdentityUserManager _userManager;
	private readonly IIdentityNotificationHistoryService _notificationHistoryService;

	private readonly int _resendTimeout;

	public TwoFactorService(
		IHttpNotificationManager notificationManager,
		IdentityUserManager userManager,
		IIdentityNotificationHistoryService notificationHistoryService,
		IOptions<TimeoutConfig> options)
	{
		_notificationManager = notificationManager;
		_userManager = userManager;
		_notificationHistoryService = notificationHistoryService;

		_resendTimeout = options.Value.SendTwoFactorCodeTimeout;
	}

	public async Task<int> SendCode(User user, bool isViaEmail = false, bool validateTimeout = false)
	{
		var channel = isViaEmail ? Channels.Email : Channels.Sms;
		var recipient = isViaEmail ? user.Email : user.PhoneNumber;

		if (validateTimeout)
		{
			await ValidateSendTimeout(user.Id);
		}

		var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);

		var notification = new TwoFactorNotification(channel)
		{
			Code = code, Username = user.UserName, Recipient = recipient, RecipientId = user.Id
		};

		await _notificationManager.SendMultiple(new[] { notification });

		return _resendTimeout;
	}

	private async Task ValidateSendTimeout(Guid userId)
	{
		var secondsTillNextSend =
			await _notificationHistoryService.GetTimeout<TwoFactorNotification>(userId, _resendTimeout);
		if (secondsTillNextSend > 0)
		{
			throw new ValidationException("Please wait for timeout. We can't resend code so often.");
		}
	}
}
