using System;
using System.Threading.Tasks;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Identity.Application.Services.NotificationHistory;

public interface IIdentityNotificationHistoryService
{
	Task<int> GetTimeout<T>(Guid recipientId, int maxTimeout)
		where T : NotificationBase;
}
