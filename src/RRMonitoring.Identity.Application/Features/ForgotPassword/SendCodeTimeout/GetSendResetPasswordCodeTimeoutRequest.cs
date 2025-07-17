using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Features.ForgotPassword.Notification;
using RRMonitoring.Identity.Application.Services.NotificationHistory;

namespace RRMonitoring.Identity.Application.Features.ForgotPassword.SendCodeTimeout;

public sealed class GetSendResetPasswordCodeTimeoutRequest : IRequest<int>
{
	public Guid RecipientId { get; init; }
}

public sealed class GetSendResetPasswordCodeTimeoutHandler : IRequestHandler<GetSendResetPasswordCodeTimeoutRequest, int>
{
	private readonly IIdentityNotificationHistoryService _notificationHistoryService;
	private readonly int _resendTimeout;

	public GetSendResetPasswordCodeTimeoutHandler(
		IIdentityNotificationHistoryService notificationHistoryService,
		IOptions<TimeoutConfig> timeoutConfig)
	{
		_notificationHistoryService = notificationHistoryService;
		_resendTimeout = timeoutConfig.Value.SendResetPasswordCodeTimeout;
	}

	public Task<int> Handle(GetSendResetPasswordCodeTimeoutRequest request, CancellationToken cancellationToken)
	{
		return _notificationHistoryService.GetTimeout<ResetPasswordNotification>(request.RecipientId, _resendTimeout);
	}
}
