using RRMonitoring.Notification.Application.Providers.Models;

namespace RRMonitoring.Notification.Application.Providers.Email.MailoPost.Models;

public sealed class MailoPostCallbackInfo : CallbackInfo
{
	public Message Message { get; set; }

	public Event Event { get; set; }
}

public sealed class Message
{
	public ulong Id { get; set; }
}

public sealed class Event
{
	public string Name { get; set; }

	public ulong Timestamp { get; set; }
}
