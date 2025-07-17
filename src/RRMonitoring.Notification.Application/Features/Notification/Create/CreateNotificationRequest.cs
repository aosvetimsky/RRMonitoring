using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Enums;
using NotificationEntry = RRMonitoring.Notification.Domain.Entities.Notification;

namespace RRMonitoring.Notification.Application.Features.Notification.Create;

public sealed class CreateNotificationRequest : IRequest
{
	public string Identifier { get; set; }
	public string Description { get; set; }
	public int GroupId { get; set; }
	public IList<CreateNotificationTemplateItem> Templates { get; set; }
}

public sealed class CreateNotificationTemplateItem
{
	public Channels ChannelId { get; set; }
	public string Subject { get; set; }
	public string Data { get; set; }
}

public sealed class CreateNotificationHandler(
	INotificationRepository notificationRepository,
	IMapper mapper)
	: IRequestHandler<CreateNotificationRequest>
{
	public async Task<Unit> Handle(CreateNotificationRequest request, CancellationToken cancellationToken)
	{
		var notification = mapper.Map<NotificationEntry>(request);

		await notificationRepository.Add(notification, cancellationToken);

		return Unit.Value;
	}
}
