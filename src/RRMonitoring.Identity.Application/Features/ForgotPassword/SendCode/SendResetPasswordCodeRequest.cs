using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Features.ForgotPassword.Notification;
using RRMonitoring.Identity.Application.Services.NotificationHistory;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;
using RRMonitoring.Notification.ApiClients.Service.Notification.Http;

namespace RRMonitoring.Identity.Application.Features.ForgotPassword.SendCode;

public sealed record SendResetPasswordCodeRequest : IRequest
{
	public User User { get; init; }

	public bool IsSendingViaEmail { get; init; }
}

public class SendResetPasswordCodeHandler : IRequestHandler<SendResetPasswordCodeRequest>
{
	private readonly IdentityUserManager _userManager;
	private readonly IHttpNotificationManager _notificationManager;
	private readonly IIdentityNotificationHistoryService _notificationHistoryService;

	private readonly int _resendTimeout;

	public SendResetPasswordCodeHandler(
		IdentityUserManager userManager,
		IHttpNotificationManager notificationManager,
		IIdentityNotificationHistoryService notificationHistoryService,
		IOptions<TimeoutConfig> options)
	{
		_userManager = userManager;
		_notificationManager = notificationManager;
		_notificationHistoryService = notificationHistoryService;

		_resendTimeout = options.Value.SendResetPasswordCodeTimeout;
	}

	public async Task<Unit> Handle(SendResetPasswordCodeRequest request, CancellationToken cancellationToken)
	{
		var token = await _userManager.GenerateUserTokenAsync(
			request.User,
			TokenOptions.DefaultPhoneProvider,
			IdentityUserManager.ResetPasswordTokenPurpose);

		var channel = request.IsSendingViaEmail ? Channels.Email : Channels.Sms;
		var recipient = request.IsSendingViaEmail ? request.User.Email : request.User.PhoneNumber;

		var secondsTillNextSend = await _notificationHistoryService
			.GetTimeout<ResetPasswordNotification>(request.User.Id, _resendTimeout);

		if (secondsTillNextSend > 0)
		{
			throw new ValidationException("Please wait for timeout. We can't resend code so often.");
		}

		var resetPasswordNotification = new ResetPasswordNotification(channel)
		{
			Recipient = recipient, RecipientId = request.User.Id, Code = token, Username = request.User.UserName
		};

		await _notificationManager.SendMultiple(new NotificationBase[] { resetPasswordNotification });

		return Unit.Value;
	}
}
