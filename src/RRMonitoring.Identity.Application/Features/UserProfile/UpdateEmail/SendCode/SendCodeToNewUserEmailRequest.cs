using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Exceptions;
using RRMonitoring.Identity.Application.Features.UserProfile.UpdateEmail.Notification;
using RRMonitoring.Identity.Application.Services.NotificationHistory;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Notification.ApiClients.Service.Notification.Http;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdateEmail.SendCode;

public sealed class SendCodeToNewUserEmailRequest : IRequest<int>
{
	public string NewEmail { get; set; }
}

internal sealed class SendCodeToNewUserEmailHandler : IRequestHandler<SendCodeToNewUserEmailRequest, int>
{
	private readonly IAccountService _accountService;
	private readonly IIdentityNotificationHistoryService _notificationHistoryService;
	private readonly IHttpNotificationManager _httpNotificationManager;
	private readonly IdentityUserManager _userManager;
	private readonly ILogger<SendCodeToNewUserEmailHandler> _logger;

	private readonly int _resendTimeout;

	public SendCodeToNewUserEmailHandler(
		IAccountService accountService,
		IIdentityNotificationHistoryService notificationHistoryService,
		IHttpNotificationManager httpNotificationManager,
		IOptions<TimeoutConfig> timeoutConfig,
		IdentityUserManager userManager,
		ILogger<SendCodeToNewUserEmailHandler> logger)
	{
		_accountService = accountService;
		_notificationHistoryService = notificationHistoryService;
		_httpNotificationManager = httpNotificationManager;
		_userManager = userManager;
		_logger = logger;

		_resendTimeout = timeoutConfig.Value.SendChangeEmailCodeTimeout;
	}

	public async Task<int> Handle(SendCodeToNewUserEmailRequest request, CancellationToken cancellationToken)
	{
		var userId = _accountService.GetCurrentUserId();
		if (!userId.HasValue)
		{
			throw new UnauthorizedAccessException();
		}

		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null)
		{
			_logger.LogError("Try to send code to email but user with ID: {UserId} was not found.", userId);

			throw new UnauthorizedAccessException();
		}

		var existingUser = await _userManager.GetByConfirmedEmail(request.NewEmail, cancellationToken);
		if (existingUser is not null)
		{
			_logger.LogWarning("User with email: {Email} already exists.", request.NewEmail);

			throw new ValidationException("Данный email уже используется.");
		}

		var secondsTillNextSend =
			await _notificationHistoryService.GetTimeout<UserConfirmEmailChangeNotification>(user.Id, _resendTimeout);
		if (secondsTillNextSend != 0)
		{
			throw new ResendNotificationTimeoutException("Please wait for timeout. We can't resend code so often",
				secondsTillNextSend);
		}

		var code = await _userManager.GenerateUserTokenAsync(
			user,
			TokenOptions.DefaultEmailProvider,
			IdentityUserManager.GetChangeEmailTokenPurpose(request.NewEmail));

		await _httpNotificationManager.SendEmail(new UserConfirmEmailChangeNotification
		{
			RecipientId = user.Id, Recipient = request.NewEmail, Code = code, Username = user.UserName
		});

		return _resendTimeout;
	}
}
