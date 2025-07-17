using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RRMonitoring.Notification.Application.Providers.Push.SignalR.Models;

namespace RRMonitoring.Notification.Application.Providers.Push.SignalR;

public interface ISignalRPushHub
{
	[HubMethodName("SendPushNotification")]
	Task SendPushNotification(SignalRPushNotification notification);
}
