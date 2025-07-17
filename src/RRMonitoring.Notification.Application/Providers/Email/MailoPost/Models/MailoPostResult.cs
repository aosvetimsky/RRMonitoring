using System.Text.Json.Serialization;

namespace RRMonitoring.Notification.Application.Providers.Email.MailoPost.Models;

internal sealed class MailoPostResult
{
	[JsonPropertyName("id")]
	public ulong Id { get; set; }

	[JsonPropertyName("payment")]
	public string Payment { get; set; }

	[JsonPropertyName("status")]
	public string Status { get; set; }
}
