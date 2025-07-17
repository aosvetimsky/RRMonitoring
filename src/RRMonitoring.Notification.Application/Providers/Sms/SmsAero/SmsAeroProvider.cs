using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RRMonitoring.Notification.Application.Configuration.Providers;
using RRMonitoring.Notification.Application.Providers.Models;
using RRMonitoring.Notification.Application.Providers.Sms.SmsAero.Enums;
using RRMonitoring.Notification.Application.Providers.Sms.SmsAero.Models;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Providers.Sms.SmsAero;

internal class SmsAeroProvider(
	HttpClient client,
	IOptions<SmsAeroProviderConfiguration> options,
	ILogger<SmsAeroProvider> logger)
	: ISmsProvider
{
	private readonly SmsAeroProviderConfiguration _options = options.Value;

	[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "By design")]
	public async Task<IList<NotificationResult>> SendNotification(NotificationInfo notificationInfo)
	{
		var notificationResult = new NotificationResult
		{
			Recipient = notificationInfo.Recipient,
			NotificationId = notificationInfo.NotificationId,
			ChannelId = (byte)Channels.Sms,
			Body = notificationInfo.Body,
			RecipientId = notificationInfo.RecipientId
		};

		logger.LogInformation(
			"Start sending {ChannelName} notification by {ProviderName} provider. Notification info: {@NotificationInfo}",
			nameof(Channels.Sms), nameof(SmsAeroProvider), notificationInfo);

		try
		{
			var authToken = Encoding.ASCII.GetBytes($"{_options.Login}:{_options.ApiKey}");
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
				Convert.ToBase64String(authToken));

			var url = await GetUrl(notificationInfo);

			var response = await client.GetAsync(url);

			if (response != null && response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<SmsAeroResult>();

				if (result is null)
				{
					throw new Exception("Failed when parse response");
				}

				if (!result.Success)
				{
					throw new Exception(result.Message);
				}

				notificationResult.ExternalMessageId = result.Data.MessageId.ToString();
				notificationResult.IsSuccess = true;
				notificationResult.Status = NotificationStatuses.Queued;
				notificationResult.ExternalStatus = result.Data.ExtendStatus;

				logger.LogInformation(
					"{ChannelName} notification with info: {@NotificationInfo} was successfully sent by {ProviderName} provider",
					nameof(Channels.Sms), notificationInfo, nameof(SmsAeroProvider));
			}
			else
			{
				var content = await response.Content.ReadAsStringAsync();

				throw new Exception($"Failed with response code: {response.StatusCode}. Content: {content}");
			}
		}
		catch (Exception ex)
		{
			logger.LogError(ex,
				"Sending {ChannelName} notification by {ProviderName} provider with info: {@NotificationInfo} was failed",
				nameof(Channels.Sms), nameof(SmsAeroProvider), notificationInfo);

			notificationResult.IsSuccess = false;
			notificationResult.Status = NotificationStatuses.Failed;
			notificationResult.Error = ex.Message;
		}

		return new List<NotificationResult> { notificationResult };
	}

	public NotificationResult ReceiveCallback(CallbackInfo callbackInfo)
	{
		if (callbackInfo is not SmsAeroCallbackInfo data)
		{
			throw new Exception($"The parameter should be {nameof(SmsAeroCallbackInfo)} type");
		}

		return new NotificationResult
		{
			ExternalMessageId = data.Id.ToString(),
			ExternalStatus = data.ExtendStatus,
			Status = MapToInternalStatus(data.Status),
		};
	}

	private async Task<string> GetUrl(NotificationInfo notificationInfo)
	{
		var message = new SmsAeroMessage
		{
			PhoneNumber = notificationInfo.Recipient,
			Text = notificationInfo.Body,
			SenderName = _options.SenderName,
			CallbackUrl = _options.CallbackUrl,
		};

		var dictFormUrlEncoded = new FormUrlEncodedContent(message.ToDictionary());

		var queryString = await dictFormUrlEncoded.ReadAsStringAsync();

		return $"{_options.Url}?{queryString}";
	}

	private static NotificationStatuses MapToInternalStatus(int externalStatus)
	{
		return externalStatus switch
		{
			(int)SmsAeroMessageStatus.Queue or (int)SmsAeroMessageStatus.OnModeration
				or (int)SmsAeroMessageStatus.Waiting
				=> NotificationStatuses.Queued,
			(int)SmsAeroMessageStatus.Delivered or (int)SmsAeroMessageStatus.Transferred => NotificationStatuses
				.Delivered,
			_ => NotificationStatuses.Failed,
		};
	}
}
