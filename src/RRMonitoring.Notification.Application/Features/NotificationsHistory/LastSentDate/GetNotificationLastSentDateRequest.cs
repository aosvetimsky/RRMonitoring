using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RRMonitoring.Notification.Domain.Contracts;

namespace RRMonitoring.Notification.Application.Features.NotificationsHistory.LastSentDate;

public sealed class GetNotificationLastSentDateRequest : IRequest<DateTime?>
{
	public string RecipientId { get; init; }
	public string NotificationIdentifier { get; init; }
}

public sealed class GetNotificationLastSentDateHandler(
	INotificationMessageHistoryRepository notificationMessageHistoryRepository)
	: IRequestHandler<GetNotificationLastSentDateRequest, DateTime?>
{
	public async Task<DateTime?> Handle(GetNotificationLastSentDateRequest request, CancellationToken cancellationToken)
	{
		return await notificationMessageHistoryRepository.GetNotificationLastSentDate(request.RecipientId,
			request.NotificationIdentifier);
	}
}
