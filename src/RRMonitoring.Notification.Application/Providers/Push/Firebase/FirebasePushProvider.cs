using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using RRMonitoring.Notification.Application.Providers.Models;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Providers.Push.Firebase;

public class FirebasePushProvider(
	IFirebaseMessagingWrapper firebaseMessagingWrapper,
	IPushRegisteredDeviceRepository pushRegisteredDeviceRepository,
	IMapper mapper,
	ILogger<FirebasePushProvider> logger)
	: IPushProvider
{
	public async Task<IList<NotificationResult>> SendNotification(NotificationInfo notificationInfo)
	{
		var recipientTokens =
			await pushRegisteredDeviceRepository
				.GetByRecipientId(notificationInfo.RecipientId ?? notificationInfo.Recipient);

		if (recipientTokens.Count == 0)
		{
			return new List<NotificationResult>();
		}

		var message = new MulticastMessage
		{
			Data = new Dictionary<string, string>
			{
				{ "NotificationId", notificationInfo.NotificationId.ToString() },
				{ "Title", notificationInfo.Subject },
				{ "Body", notificationInfo.Body }
			}
		};

		var tokensToDelete = new List<PushRegisteredDevice>();

		var notificationResults = new List<NotificationResult>();

		for (var i = 0; i < recipientTokens.Count; i += 500)
		{
			message.Tokens = recipientTokens.Skip(i)
				.Take(500)
				.Select(x => x.Token)
				.ToList();

			logger.LogInformation(
				"Start sending  notification by FirebasePush provider. Notification info: {@NotificationInfo}",
				notificationInfo);

			var response = await firebaseMessagingWrapper.SendMulticastAsync(message);

			for (var j = 0; j < response.Responses.Count; j++)
			{
				var sendResponse = response.Responses[j];

				var newNotificationResult = mapper.Map<NotificationResult>(notificationInfo);

				newNotificationResult.ExternalMessageId = GetExternalMessageId(sendResponse.MessageId);
				newNotificationResult.IsSuccess = sendResponse.IsSuccess;
				newNotificationResult.Error = sendResponse.IsSuccess ? null : sendResponse.Exception.Message;
				newNotificationResult.Status =
					sendResponse.IsSuccess ? NotificationStatuses.Queued : NotificationStatuses.Failed;
				newNotificationResult.ChannelId = (byte)Channels.Push;

				if (newNotificationResult.IsSuccess)
				{
					logger.LogInformation(
						"Notification with info: {@NotificationInfo} was successfully sent by FirebasePush provider for {Token}",
						notificationInfo, message.Tokens[j]);
				}
				else
				{
					logger.LogError(sendResponse.Exception,
						"Sending notification for {Token} by FirebasePush provider with info: {@NotificationInfo} was failed",
						message.Tokens[j], notificationInfo);
				}

				newNotificationResult.Recipient = message.Tokens[j];

				notificationResults.Add(newNotificationResult);

				// if device wasn't found in Firebase we should delete token for future sendings
				if (sendResponse.Exception != null &&
				    (sendResponse.Exception.ErrorCode == ErrorCode.NotFound ||
				     sendResponse.Exception.ErrorCode == ErrorCode.InvalidArgument))
				{
					tokensToDelete.Add(
						recipientTokens.First(x => x.Token == newNotificationResult.Recipient));
				}
			}
		}

		if (tokensToDelete.Any())
		{
			await pushRegisteredDeviceRepository.RemoveRange(tokensToDelete.ToArray());
		}

		return notificationResults;
	}

	public NotificationResult ReceiveCallback(CallbackInfo callbackInfo)
	{
		throw new NotImplementedException();
	}

	private static string GetExternalMessageId(string messageId)
	{
		return messageId?.Split('/')
			.Last()
			.Trim();
	}
}
