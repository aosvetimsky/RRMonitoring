using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Notification.Domain.Contracts;
using NotificationEntry = RRMonitoring.Notification.Domain.Entities.Notification;

namespace RRMonitoring.Notification.Application.Features.Notification.GetById;

public sealed class GetNotificationByIdRequest(Guid id) : IRequest<NotificationInfoResponse>
{
	public Guid Id { get; set; } = id;
}

public sealed class GetNotificationByIdHandler(
	INotificationRepository notificationRepository,
	IMapper mapper)
	: IRequestHandler<GetNotificationByIdRequest, NotificationInfoResponse>
{
	public async Task<NotificationInfoResponse> Handle(
		GetNotificationByIdRequest request, CancellationToken cancellationToken)
	{
		var includes = new[] { nameof(NotificationEntry.Templates) };
		var notification =
			await notificationRepository.GetById(request.Id, includes, asNoTracking: true, cancellationToken);

		return mapper.Map<NotificationInfoResponse>(notification);
	}
}
