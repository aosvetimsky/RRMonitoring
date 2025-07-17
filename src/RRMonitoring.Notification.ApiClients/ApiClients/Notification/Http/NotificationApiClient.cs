using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RRMonitoring.Notification.ApiClients.ApiClients.Notification.Models;
using RRMonitoring.Notification.ApiClients.Configuration;
using RRMonitoring.Notification.ApiClients.Models;

namespace RRMonitoring.Notification.ApiClients.ApiClients.Notification.Http;

internal class NotificationApiClient(
	HttpClient client,
	IOptions<NotificationHttpConfiguration> options)
	: INotificationApiClient
{
	private const string ApiKeyHeaderName = "X-Api-Key";

	private readonly NotificationHttpConfiguration _notificationHttpConfiguration = options.Value;

	public async Task SendMultiple(IList<SendNotificationRequest> requests)
	{
		var content = await GetMessageContentAsync(requests);

		content.Headers.Add(ApiKeyHeaderName, _notificationHttpConfiguration.ApiKey);

		var response = await client.PostAsync(_notificationHttpConfiguration.Url + "/v1/notification/send-multiple",
			content);

		response.EnsureSuccessStatusCode();
	}

	public async Task<DateTime?> GetNotificationLastSentDate<TNotificationBase>(Guid recipientId)
		where TNotificationBase : NotificationBase
	{
		var bodyContent = new
		{
			RecipientId = recipientId.ToString(), NotificationIdentifier = typeof(TNotificationBase).Name
		};

		var bodyBuffer = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(bodyContent));
		using var content = new ByteArrayContent(bodyBuffer);

		content.Headers.Add(ApiKeyHeaderName, _notificationHttpConfiguration.ApiKey);
		content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

		var response =
			await client.PostAsync(_notificationHttpConfiguration.Url + "/v1/notification-history/last-sent-date",
				content);

		response.EnsureSuccessStatusCode();

		var lastSentDate = await response.Content.ReadAsStringAsync();

		if (string.IsNullOrWhiteSpace(lastSentDate))
		{
			return null;
		}

		return Convert.ToDateTime(lastSentDate.Trim('\"')).ToUniversalTime();
	}

	private static async Task<MultipartFormDataContent> GetMessageContentAsync(IList<SendNotificationRequest> requests)
	{
		var content = new MultipartFormDataContent();

		for (var i = 0; i < requests.Count; i++)
		{
			content.Add(new StringContent(requests[i].Identifier), $"NotificationRequests[{i}].Identifier");
			content.Add(new StringContent(requests[i].Recipient), $"NotificationRequests[{i}].Recipient");
			content.Add(new StringContent(requests[i].RecipientId.ToString()), $"NotificationRequests[{i}].RecipientId");
			content.Add(new StringContent(requests[i].Channel.ToString()), $"NotificationRequests[{i}].Channel");
			content.Add(new StringContent(requests[i].UserData), $"NotificationRequests[{i}].UserData");

			if (requests[i].Files?.Any() == true)
			{
				foreach (var file in requests[i].Files)
				{
					var stream = new MemoryStream();
					await file.CopyToAsync(stream);

					content.Add(new ByteArrayContent(stream.ToArray()), $"NotificationRequests[{i}].Attachments",
						file.FileName);
				}
			}
		}

		return content;
	}
}
