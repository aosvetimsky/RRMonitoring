using RRMonitoring.Notification.ApiClients.ApiClients.Notification.Rabbit;

namespace RRMonitoring.Notification.ApiClients.Service.Notification.Rabbit;

internal class RabbitNotificationManager : NotificationManager, IRabbitNotificationManager
{
	public RabbitNotificationManager(INotificationRabbitProducer notificationRabbitProducer)
		: base(notificationRabbitProducer)
	{
	}
}
