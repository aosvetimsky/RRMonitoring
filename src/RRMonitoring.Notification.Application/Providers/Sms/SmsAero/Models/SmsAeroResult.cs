using System.Text.Json.Serialization;

namespace RRMonitoring.Notification.Application.Providers.Sms.SmsAero.Models;

internal sealed class SmsAeroResult
{
	public bool Success { get; set; }

	public SmsAeroMessageData Data { get; set; }

	public string Message { get; set; }
}

internal sealed class SmsAeroMessageData
{
	[JsonPropertyName("id")]
	public int MessageId { get; set; }

	[JsonPropertyName("number")]
	public string PhoneNumber { get; set; }

	public string Text { get; set; }

	public int Status { get; set; }

	public string ExtendStatus { get; set; }
}
