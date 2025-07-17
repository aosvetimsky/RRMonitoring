using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RRMonitoring.Notification.Application.Providers.Push.SignalR;

[Authorize]
public class SignalRPushHub(ISignalRUsersService signalRUsersService)
	: Hub<ISignalRPushHub>
{
	public override Task OnConnectedAsync()
	{
		signalRUsersService.AddUser(Context.UserIdentifier);

		return base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception exception)
	{
		signalRUsersService.RemoveUser(Context.UserIdentifier);

		return base.OnDisconnectedAsync(exception);
	}
}
