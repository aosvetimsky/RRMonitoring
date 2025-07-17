using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RRMonitoring.Notification.Application.Providers.Models;
using RRMonitoring.Notification.Application.Services.Notification;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Features.NotificationMessages.ReceiveCallback;

public class ReceiveCallbackRequest : IRequest
{
	public CallbackInfo CallbackInfo { get; set; }
	public Channels Channel { get; set; }
}

public class ReceiveCallbackHandler(
	ProviderFactory providerFactory,
	INotificationService notificationService,
	INotificationMessageHistoryRepository notificationMessageHistoryRepository)
	: IRequestHandler<ReceiveCallbackRequest>
{
	public async Task<Unit> Handle(ReceiveCallbackRequest request, CancellationToken cancellationToken)
	{
		var provider = providerFactory.GetProvider(request.Channel);
		var result = provider.ReceiveCallback(request.CallbackInfo);

		var message = await notificationService
			.GetRequiredMessageByExternalId(request.Channel, result.ExternalMessageId, cancellationToken);

		var newHistoryEntry = new NotificationMessageHistory
		{
			NotificationMessageId = message.Id,
			StatusId = (byte)result.Status,
			ExternalSystemStatus = result.ExternalStatus
		};

		await notificationMessageHistoryRepository.Add(newHistoryEntry, cancellationToken);

		return Unit.Value;
	}
}
