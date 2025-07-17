using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Exceptions;
using RRMonitoring.Identity.Application.Extensions;
using RRMonitoring.Identity.Application.Features.UserProfile.UpdatePhone.Notification;
using RRMonitoring.Identity.Application.Services.NotificationHistory;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Notification.ApiClients.Service.Notification.Http;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdatePhone.SendCode;

public record SendCodeToNewPhoneRequest(string NewPhoneNumber) : IRequest<int>;

public class SendCodeToNewPhoneHandler : IRequestHandler<SendCodeToNewPhoneRequest, int>
{
	private readonly IAccountService _accountService;
	private readonly IdentityUserManager _userManager;
	private readonly IHttpNotificationManager _httpNotificationManager;
	private readonly IIdentityNotificationHistoryService _notificationHistoryService;
	private readonly ILogger<SendCodeToNewPhoneHandler> _logger;

	private readonly int _resendTimeout;

	public SendCodeToNewPhoneHandler(
		IAccountService accountService,
		IIdentityNotificationHistoryService notificationHistoryService,
		IHttpNotificationManager httpNotificationManager,
		IdentityUserManager userManager,
		ILogger<SendCodeToNewPhoneHandler> logger,
		IOptions<TimeoutConfig> timeoutConfig)
	{
		_accountService = accountService;
		_userManager = userManager;
		_httpNotificationManager = httpNotificationManager;
		_notificationHistoryService = notificationHistoryService;
		_logger = logger;

		_resendTimeout = timeoutConfig.Value.SendChangePhoneCodeTimeout;
	}

	public async Task<int> Handle(SendCodeToNewPhoneRequest request, CancellationToken cancellationToken)
	{
		var userId = _accountService.GetRequiredCurrentUserId();
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null)
		{
			_logger.LogError("Try to send code to phone but user with ID: {UserId} was not found.", userId);

			throw new UnauthorizedAccessException();
		}

		var existingUser = await _userManager.GetByConfirmedPhoneNumber(request.NewPhoneNumber, cancellationToken);
		if (existingUser is not null)
		{
			_logger.LogWarning("User with phone: {PhoneNumber} already exists.", request.NewPhoneNumber);

			throw new ValidationException("Данный номер телефона уже используется.");
		}

		var secondsTillNextSend =
			await _notificationHistoryService
				.GetTimeout<UserConfirmPhoneChangeSmsNotification>(user.Id, _resendTimeout);
		if (secondsTillNextSend != 0)
		{
			throw new ResendNotificationTimeoutException("Please wait for timeout. We can't resend code so often",
				secondsTillNextSend);
		}

		var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, request.NewPhoneNumber);

		await _httpNotificationManager.SendSms(new UserConfirmPhoneChangeSmsNotification
		{
			Recipient = request.NewPhoneNumber, RecipientId = user.Id, Code = code
		});

		return _resendTimeout;
	}
}
