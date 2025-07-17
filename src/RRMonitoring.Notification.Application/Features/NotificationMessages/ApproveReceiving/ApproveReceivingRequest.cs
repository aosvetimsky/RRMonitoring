using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RRMonitoring.Notification.Application.Services.Notification;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Features.NotificationMessages.ApproveReceiving;

public class ApproveReceivingRequest : IRequest
{
	public string ExternalMessageId { get; set; }
}

public class ApproveReceivingHandler(
	INotificationService notificationService,
	INotificationMessageHistoryRepository notificationMessageHistoryRepository)
	: IRequestHandler<ApproveReceivingRequest>
{
	public async Task<Unit> Handle(ApproveReceivingRequest request, CancellationToken cancellationToken)
	{
		var message = await notificationService
			.GetRequiredMessageByExternalId(Channels.Push, request.ExternalMessageId, cancellationToken);

		var newHistoryEntry = new NotificationMessageHistory
		{
			NotificationMessageId = message.Id,
			StatusId = (byte)NotificationStatuses.Delivered
		};

		await notificationMessageHistoryRepository.Add(newHistoryEntry, cancellationToken);

		return Unit.Value;
	}
}
