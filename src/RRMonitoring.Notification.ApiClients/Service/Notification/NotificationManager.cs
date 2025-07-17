using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RRMonitoring.Notification.ApiClients.ApiClients.Notification;
using RRMonitoring.Notification.ApiClients.ApiClients.Notification.Models;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Notification.ApiClients.Service.Notification;

internal abstract class NotificationManager(INotificationProvider notificationProvider)
	: INotificationManager
{
	public virtual async Task SendEmail(EmailNotification notification)
	{
		await SendMultiple(new[] { notification });
	}

	public virtual async Task SendSms(SmsNotification notification)
	{
		await SendMultiple(new[] { notification });
	}

	public virtual async Task SendPush(PushNotification notification)
	{
		await SendMultiple(new[] { notification });
	}

	public virtual async Task SendMultiple(IList<NotificationBase> notifications)
	{
		if (notifications?.Any() != true)
		{
			throw new ArgumentException("Notifications are empty array");
		}

		if (notifications.Any(x => !Enum.IsDefined(x.Channel)))
		{
			throw new ArgumentException("There are Unknown channel in notifications");
		}

		var requests = notifications.Select(MapToNotificationRequest).ToList();

		await notificationProvider.SendMultiple(requests);
	}

	protected virtual SendNotificationRequest MapToNotificationRequest(NotificationBase notification)
	{
		return new SendNotificationRequest
		{
			Identifier = notification.GetType().Name,
			RecipientId = notification.RecipientId,
			Recipient = notification.Recipient,
			Channel = notification.Channel,
			UserData = JsonConvert.SerializeObject(notification)
		};
	}
}
