using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Notification.Application.Features.Notification.Send;
using RRMonitoring.Notification.Application.Features.Notification.SendManual;
using RRMonitoring.Notification.Application.Providers.Models;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Services.Notification;

internal class NotificationService(
	ProviderFactory providerFactory,
	IAccountService accountService,
	INotificationRepository notificationRepository,
	INotificationMessageRepository notificationMessageRepository,
	INotificationMessageHistoryRepository notificationMessageHistoryRepository,
	IRecipientSettingsRepository recipientSettingsRepository,
	ITemplateRepository templateRepository,
	IMapper mapper)
	: INotificationService
{
	public async Task Send(SendNotificationRequest notificationRequest)
	{
		var notificationInfo = await GetNotificationInfo(notificationRequest);

		var canSendNotification = notificationRequest.RecipientId == null
			|| await recipientSettingsRepository.IsNotificationEnabledForRecipient(notificationRequest.RecipientId, notificationInfo.Identifier);

		if (canSendNotification)
		{
			await Send(notificationInfo, notificationRequest.Channel);
		}
	}

	public async Task Send(SendManualNotificationRequest notificationRequest)
	{
		var notificationsInfo = GetNotificationInfo(notificationRequest);

		foreach (var notificationInfo in notificationsInfo)
		{
			await Send(notificationInfo, notificationRequest.Channel);
		}
	}

	public async Task<NotificationMessage> GetRequiredMessageByExternalId(
		Channels channel,
		string externalMessageId,
		CancellationToken cancellationToken)
	{
		var message = await notificationMessageRepository.GetByExternalId(channel, externalMessageId, cancellationToken);

		if (message == null)
		{
			throw new ValidationException($"{channel} message with ExternalMessageId {externalMessageId} was not found.");
		}

		return message;
	}

	private async Task Send(NotificationInfo notificationInfo, Channels channel)
	{
		var provider = providerFactory.GetProvider(channel);
		var notificationResult = await provider.SendNotification(notificationInfo);

		await SaveHistory(notificationResult);
	}

	private async Task SaveHistory(IEnumerable<NotificationResult> notificationResults)
	{
		var currentUserId = GetCurrentUseId() ?? Guid.Empty;

		var notificationHistory = notificationResults
			.Select(notificationResult =>
			{
				var result = mapper.Map<NotificationMessageHistory>(notificationResult);
				result.CreatedBy = currentUserId;
				result.NotificationMessage = mapper.Map<NotificationMessage>(notificationResult);
				result.NotificationMessage.CreatedBy = currentUserId;

				return result;
			})
			.ToList();

		await notificationMessageHistoryRepository.AddRange(notificationHistory);
	}

	private Guid? GetCurrentUseId()
	{
		try
		{
			return accountService.GetCurrentUserId(); // TODO: Fix AccountService.GetCurrentUserId()
		}
		catch (NullReferenceException)
		{
			return null;
		}
		catch (AuthenticationException)
		{
			return null;
		}
	}

	private async Task<NotificationInfo> GetNotificationInfo(SendNotificationRequest notificationRequest)
	{
		var notification = await notificationRepository.GetByIdentifier(notificationRequest.Identifier);
		if (notification == null)
		{
			throw new ValidationException($"Notification with Identifier: {notificationRequest.Identifier} doesn't exist");
		}

		var template = await templateRepository.Get(notificationRequest.Channel, notification.Id);

		var renderedTemplate = template != null
			? await TemplateRenderer.Render(template.Data, notificationRequest.UserData)
			: notificationRequest.UserData;

		return new NotificationInfo
		{
			NotificationId = notification.Id,
			Identifier = notificationRequest.Identifier,
			RecipientId = notificationRequest.RecipientId,
			Recipient = notificationRequest.Recipient,
			Subject = template?.Subject ?? string.Empty,
			Body = renderedTemplate ?? string.Empty,
			Attachments = notificationRequest.Attachments
		};
	}

	private static IEnumerable<NotificationInfo> GetNotificationInfo(SendManualNotificationRequest notificationRequest)
	{
		return notificationRequest.Recipients
			.Select(recipient => new NotificationInfo
			{
				Recipient = recipient,
				Subject = notificationRequest.Subject,
				Body = notificationRequest.Text,
				Attachments = notificationRequest.Attachments
			})
			.ToList();
	}
}
