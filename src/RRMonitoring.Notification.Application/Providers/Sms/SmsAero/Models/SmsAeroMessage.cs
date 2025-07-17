using System.Collections.Generic;

namespace RRMonitoring.Notification.Application.Providers.Sms.SmsAero.Models;

internal class SmsAeroMessage
{
	public string PhoneNumber { get; set; }

	public string Text { get; set; }

	public string SenderName { get; set; }

	public string CallbackUrl { get; set; }

	public Dictionary<string, string> ToDictionary()
	{
		return new Dictionary<string, string>
		{
			{ "number", PhoneNumber },
			{ "text", Text },
			{ "sign", SenderName },
			{ "callbackUrl", CallbackUrl},
		};
	}
}
