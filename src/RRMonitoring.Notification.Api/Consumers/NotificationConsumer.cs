using System.Threading.Tasks;
using MassTransit;
using MediatR;
using RRMonitoring.Notification.Application.Features.Notification.Send;
using RRMonitoring.Notification.BusEvents.Notifications;
using Channels = RRMonitoring.Notification.Domain.Enums.Channels;

namespace RRMonitoring.Notification.Api.Consumers;

public class NotificationConsumer(IMediator mediator) : IConsumer<SendNotificationEvent>
{
	public async Task Consume(ConsumeContext<SendNotificationEvent> context)
	{
		foreach (var item in context.Message.Items)
		{
			var request = MapToRequest(item);
			await mediator.Send(request);
		}
	}

	private static SendNotificationRequest MapToRequest(NotificationEventItem eventItem)
	{
		return new SendNotificationRequest
		{
			Identifier = eventItem.Identifier,
			Recipient = eventItem.Recipient,
			RecipientId = eventItem.RecipientId,
			Channel = (Channels)eventItem.Channel,
			UserData = eventItem.UserData
		};
	}
}
