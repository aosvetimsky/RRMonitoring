using System.Text.Json.Serialization;

namespace RRMonitoring.Notification.Application.Providers.Email.MailoPost.Models;

internal sealed class MailoPostMessage
{
	[JsonPropertyName("from_email")]
	public string Sender { get; set; }

	[JsonPropertyName("to")]
	public string Recipient { get; set; }

	[JsonPropertyName("subject")]
	public string Subject { get; set; }

	[JsonPropertyName("html")]
	public string Html { get; set; }

	[JsonPropertyName("payment")]
	public string Payment { get; set; }
}
