namespace RRMonitoring.Notification.Domain.Entities;

public class PushRegisteredDevice
{
	public string RecipientId { get; set; }
	public string DeviceId { get; set; }
	public string Token { get; set; }
}
