using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Notification.Domain.Contracts;

namespace RRMonitoring.Notification.Application.Features.Notification.Delete;

public sealed class DeleteNotificationRequest(Guid id) : IRequest
{
	public Guid Id { get; set; } = id;
}

public sealed class DeleteNotificationHandler(INotificationRepository notificationRepository)
	: IRequestHandler<DeleteNotificationRequest>
{
	public async Task<Unit> Handle(DeleteNotificationRequest request, CancellationToken cancellationToken)
	{
		var notification = await notificationRepository
			.GetById(request.Id, asNoTracking: true, cancellationToken: cancellationToken);

		if (notification is null)
		{
			throw new ValidationException($"Notification with ID: {request.Id} was not found");
		}

		await notificationRepository.Delete(notification, cancellationToken);

		return Unit.Value;
	}
}
