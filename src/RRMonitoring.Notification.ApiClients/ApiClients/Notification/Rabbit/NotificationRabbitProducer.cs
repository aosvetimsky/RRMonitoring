using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using RRMonitoring.Notification.ApiClients.ApiClients.Notification.Models;
using RRMonitoring.Notification.BusEvents.Notifications;

namespace RRMonitoring.Notification.ApiClients.ApiClients.Notification.Rabbit;

internal class NotificationRabbitProducer(IPublishEndpoint publishEndpoint) : INotificationRabbitProducer
{
	public async Task SendMultiple(IList<SendNotificationRequest> requests)
	{
		if (requests.Any(x => x.Files?.Any() == true))
		{
			throw new ArgumentException("Cannot send files using RabbitMQ");
		}

		var busEvent = MapToEvent(requests);
		await publishEndpoint.Publish(busEvent);
	}

	private static SendNotificationEvent MapToEvent(IList<SendNotificationRequest> requests)
	{
		return new SendNotificationEvent
		{
			Items = requests
				.Select(x => new NotificationEventItem
				{
					Identifier = x.Identifier,
					Channel = (Channels)x.Channel,
					Recipient = x.Recipient,
					RecipientId = x.RecipientId.ToString(),
					UserData = x.UserData
				})
				.ToList()
		};
	}
}
