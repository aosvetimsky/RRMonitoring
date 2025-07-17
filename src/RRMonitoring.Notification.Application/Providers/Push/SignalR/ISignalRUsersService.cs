namespace RRMonitoring.Notification.Application.Providers.Push.SignalR;

public interface ISignalRUsersService
{
	void AddUser(string userId);

	void RemoveUser(string userId);

	bool IsUserConnected(string userId);
}
