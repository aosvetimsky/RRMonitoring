using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RRMonitoring.Notification.Application.Configuration.Providers;
using RRMonitoring.Notification.Application.Providers.Email.MailoPost.Models;
using RRMonitoring.Notification.Application.Providers.Models;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Providers.Email.MailoPost;

internal sealed class MailoPostEmailProvider(
	HttpClient client,
	IOptions<MailoPostProviderConfiguration> options,
	ILogger<MailoPostEmailProvider> logger)
	: IEmailProvider
{
	private readonly MailoPostProviderConfiguration _options = options.Value;

	[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "By design")]
	public async Task<IList<NotificationResult>> SendNotification(NotificationInfo notificationInfo)
	{
		var emailMessageForm = await GetMessageContent(notificationInfo);

		var notificationResult = new NotificationResult
		{
			Recipient = notificationInfo.Recipient,
			NotificationId = notificationInfo.NotificationId,
			ChannelId = (byte)Channels.Email,
			Body = notificationInfo.Body,
			RecipientId = notificationInfo.RecipientId
		};

		logger.LogInformation(
			"Start sending email notification by MailoPost provider. Notification info: {@NotificationInfo}",
			notificationInfo);

		try
		{
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.Token);

			var response = await client.PostAsync(_options.Url, emailMessageForm);

			if (response?.IsSuccessStatusCode == true)
			{
				var result = await response.Content.ReadFromJsonAsync<MailoPostResult>();

				if (result is null)
				{
					throw new Exception("Failed when parse response");
				}

				notificationResult.ExternalMessageId = result.Id.ToString();
				notificationResult.IsSuccess = true;
				notificationResult.Status = NotificationStatuses.Queued;
				notificationResult.ExternalStatus = result.Status;

				logger.LogInformation(
					"Email notification with info: {@NotificationInfo} was successfully sent by MailoPost provider",
					notificationInfo);
			}
			else
			{
				var responseMessage = await response.Content.ReadAsStringAsync();

				throw new Exception($"Failed with response: {responseMessage}");
			}
		}
		catch (Exception ex)
		{
			logger.LogError(ex,
				"Sending email notification by MailoPost provider with info: {@NotificationInfo} was failed",
				notificationInfo);

			notificationResult.IsSuccess = false;
			notificationResult.Status = NotificationStatuses.Failed;
			notificationResult.Error = ex.Message;
		}

		return new List<NotificationResult> { notificationResult };
	}

	public NotificationResult ReceiveCallback(CallbackInfo callbackInfo)
	{
		if (callbackInfo is not MailoPostCallbackInfo data)
		{
			throw new Exception($"The parameter should be {nameof(MailoPostCallbackInfo)} type");
		}

		return new NotificationResult
		{
			ExternalMessageId = data.Message.Id.ToString(),
			ExternalStatus = data.Event.Name,
			Status = MapToInternalStatus(data.Event.Name),
		};
	}

	private static NotificationStatuses MapToInternalStatus(string externalStatus)
	{
		return externalStatus switch
		{
			"delivered" or "opened" or "clicked" => NotificationStatuses.Delivered,
			_ => NotificationStatuses.Failed,
		};
	}

	private async Task<MultipartFormDataContent> GetMessageContent(NotificationInfo notificationInfo)
	{
		var emailMessageForm = new MultipartFormDataContent();

		var subject = string.IsNullOrEmpty(notificationInfo.Subject)
			? "Без темы"
			: notificationInfo.Subject;

		emailMessageForm.Add(new StringContent(_options.SenderEmail), "from_email");
		emailMessageForm.Add(new StringContent(notificationInfo.Recipient), "to");
		emailMessageForm.Add(new StringContent(subject), "subject");
		emailMessageForm.Add(new StringContent(notificationInfo.Body), "html");
		emailMessageForm.Add(new StringContent(_options.Payment), "payment");

		if (notificationInfo.Attachments != null)
		{
			foreach (var file in notificationInfo.Attachments)
			{
				using var stream = new MemoryStream();
				await file.CopyToAsync(stream);

				emailMessageForm.Add(new ByteArrayContent(stream.ToArray()), "attachments[]", file.FileName);
			}
		}

		return emailMessageForm;
	}
}
