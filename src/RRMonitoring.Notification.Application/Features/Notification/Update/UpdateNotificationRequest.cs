using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Enums;
using NotificationEntry = RRMonitoring.Notification.Domain.Entities.Notification;


namespace RRMonitoring.Notification.Application.Features.Notification.Update;

public sealed class UpdateNotificationRequest : IRequest
{
	[JsonIgnore]
	public Guid Id { get; set; }

	public string Identifier { get; set; }
	public string Description { get; set; }
	public int GroupId { get; set; }
	public IList<UpdateNotificationTemplateItem> Templates { get; set; }
}

public sealed class UpdateNotificationTemplateItem
{
	public Guid? Id { get; set; }
	public Channels ChannelId { get; set; }
	public string Subject { get; set; }
	public string Data { get; set; }
}

public sealed class UpdateNotificationHandler(
	INotificationRepository notificationRepository,
	IMapper mapper)
	: IRequestHandler<UpdateNotificationRequest>
{
	public async Task<Unit> Handle(UpdateNotificationRequest request, CancellationToken cancellationToken)
	{
		var includes = new[] { nameof(NotificationEntry.Templates) };
		var notification = await notificationRepository
			.GetById(request.Id, includes, asNoTracking: true, cancellationToken);

		if (notification is null)
		{
			throw new ValidationException($"Notification with ID: {request.Id} was not found");
		}

		mapper.Map(request, notification);

		await notificationRepository.Update(notification, cancellationToken);

		return Unit.Value;
	}
}
