using System.Collections.Generic;

namespace RRMonitoring.Notification.Application.Providers.Push.SignalR;

internal class SignalRUsersService : ISignalRUsersService
{
	private readonly object _lock = new();
	private readonly HashSet<string> _connectedUsers = [];

	public void AddUser(string userId)
	{
		lock (_lock)
		{
			_connectedUsers.Add(userId);
		}
	}

	public void RemoveUser(string userId)
	{
		lock (_lock)
		{
			_connectedUsers.Remove(userId);
		}
	}

	public bool IsUserConnected(string userId)
	{
		lock (_lock)
		{
			return _connectedUsers.Contains(userId);
		}
	}
}
