using System.Threading;
using System.Threading.Tasks;
using RRMonitoring.Notification.Application.Features.Notification.Send;
using RRMonitoring.Notification.Application.Features.Notification.SendManual;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Services.Notification;

public interface INotificationService
{
	Task Send(SendNotificationRequest notificationRequest);

	Task Send(SendManualNotificationRequest notificationRequest);

	Task<NotificationMessage> GetRequiredMessageByExternalId(
		Channels channel,
		string externalMessageId,
		CancellationToken cancellationToken);
}
