using System.Text.Json.Serialization;

namespace RRMonitoring.Notification.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Channels
{
	Email = 1,
	Push = 2,
	Sms = 3
}
