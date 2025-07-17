using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RRMonitoring.Notification.Application.Providers.Models;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Providers.Push.SignalR;

public class SignalRPushProvider(
	ISignalRUsersService signalRUsersService,
	IHubContext<SignalRPushHub, ISignalRPushHub> hubContext,
	IMapper mapper,
	ILogger<SignalRPushProvider> logger)
	: IPushProvider
{
	public async Task<IList<NotificationResult>> SendNotification(NotificationInfo notificationInfo)
	{
		if (!signalRUsersService.IsUserConnected(notificationInfo.RecipientId ?? notificationInfo.Recipient))
		{
			return Array.Empty<NotificationResult>();
		}

		var notificationResult = mapper.Map<NotificationResult>(notificationInfo);
		notificationResult.ChannelId = (byte)Channels.Push;

		logger.LogInformation(
			"Start sending {ChannelName} notification by {ProviderName} provider. Notification info: {@NotificationInfo}",
			nameof(Channels.Push), nameof(SignalRPushProvider), notificationInfo);

		try
		{
			var user = hubContext.Clients.User(notificationInfo.RecipientId ?? notificationInfo.Recipient);

			await user.SendPushNotification(new() { Subject = notificationInfo.Subject, Body = notificationInfo.Body });

			notificationResult.IsSuccess = true;
			notificationResult.Status = NotificationStatuses.Delivered;
		}
#pragma warning disable CA1031
		catch (Exception ex)
#pragma warning restore CA1031
		{
			logger.LogError(ex,
				"Sending {ChannelName} notification by {ProviderName} provider with info: {@NotificationInfo} was failed",
				nameof(Channels.Push), nameof(SignalRPushProvider), notificationInfo);

			notificationResult.IsSuccess = false;
			notificationResult.Status = NotificationStatuses.Failed;
			notificationResult.Error = ex.Message;
		}

		return new List<NotificationResult> { notificationResult };
	}

	public NotificationResult ReceiveCallback(CallbackInfo callbackInfo)
	{
		throw new NotImplementedException();
	}
}
