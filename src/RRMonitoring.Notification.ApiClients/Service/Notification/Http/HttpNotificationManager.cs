using RRMonitoring.Notification.ApiClients.ApiClients.Notification.Http;

#pragma warning disable IDE0130 // Namespace does not match folder structure // TODO: Fix namespace
namespace RRMonitoring.Notification.ApiClients.Service.Notification.Http;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal class HttpNotificationManager : NotificationManager, IHttpNotificationManager
{
	public HttpNotificationManager(INotificationApiClient notificationApiClient)
		: base(notificationApiClient)
	{
	}
}
