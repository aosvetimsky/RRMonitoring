using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using RRMonitoring.Notification.Application.Configuration;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Features.RecipientSettings.AddDefault;

public record AddDefaultUserSettingsRequest(Guid UserId) : IRequest;

public class AddDefaultUserSettingsHandler(
	IRecipientSettingsRepository recipientSettingsRepository,
	IOptions<DefaultRecipientSettingsConfiguration> options)
	: IRequestHandler<AddDefaultUserSettingsRequest>
{
	private readonly Dictionary<string, DefaultSettingState> _notificationDefaultSettings = options.Value.Values;

	public async Task<Unit> Handle(AddDefaultUserSettingsRequest request, CancellationToken cancellationToken)
	{
		var userSettings = await recipientSettingsRepository.GetByRecipientId(request.UserId.ToString());

		var existingEmailNotifications = userSettings
			.Where(x => x.ChannelId == (byte)Channels.Email)
			.Select(x => x.NotificationIdentifier);

		var existingPhoneNotifications = userSettings
			.Where(x => x.ChannelId == (byte)Channels.Sms)
			.Select(x => x.NotificationIdentifier);

		var requiredEmailNotifications = _notificationDefaultSettings
			.Where(x => x.Value.IsEmailEnabled.HasValue);

		var requiredPhoneNotifications = _notificationDefaultSettings
			.Where(x => x.Value.IsPhoneEnabled.HasValue);

		var emailNotificationsToAdd = requiredEmailNotifications
			.Where(x => !existingEmailNotifications.Contains(x.Key))
			.ToList();
		var phoneNotificationsToAdd = requiredPhoneNotifications
			.Where(x => !existingPhoneNotifications.Contains(x.Key))
			.ToList();

		var settingsToSave = GetRequiredSettings(request.UserId, emailNotificationsToAdd, phoneNotificationsToAdd);

		await recipientSettingsRepository.SetRange(settingsToSave);

		return Unit.Value;
	}

	private static List<RecipientSetting> GetRequiredSettings(
		Guid userId,
		List<KeyValuePair<string, DefaultSettingState>> emailNotificationToAdd,
		List<KeyValuePair<string, DefaultSettingState>> phoneNotificationToAdd)
	{
		var emailSettings = emailNotificationToAdd
			.Select(x => new RecipientSetting
			{
				NotificationIdentifier = x.Key,
				RecipientId = userId.ToString(),
				ChannelId = (byte)Channels.Email,
				IsEnabled = x.Value.IsEmailEnabled ?? false,
			});

		var phoneSettings = phoneNotificationToAdd
			.Select(x => new RecipientSetting
			{
				NotificationIdentifier = x.Key,
				RecipientId = userId.ToString(),
				ChannelId = (byte)Channels.Sms,
				IsEnabled = x.Value.IsPhoneEnabled ?? false
			});

		return emailSettings.Concat(phoneSettings).ToList();
	}
}
