using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;
using ValidationException = Nomium.Core.Exceptions.ValidationException;

namespace RRMonitoring.Notification.Application.Features.RecipientSettings.SetCurrentUserRecipientSettings;

public class SetUserSettingsRequest : IRequest
{
	[Required]
	public string Identifier { get; init; }

	[Required]
	public Channels Channel { get; init; }

	[Required]
	public bool IsEnabled { get; init; }
}

public class SetUserSettingsRequestHandler(
	IRecipientSettingsRepository recipientSettingsRepository,
	IAccountService accountService)
	: IRequestHandler<SetUserSettingsRequest>
{
	public async Task<Unit> Handle(SetUserSettingsRequest request, CancellationToken cancellationToken)
	{
		var userId = accountService.GetCurrentUserId();
		if (userId is null)
		{
			throw new UnauthorizedAccessException();
		}

		var recipientSettings = await recipientSettingsRepository.GetByRecipientId(userId.ToString());
		var settingToUpdate = recipientSettings
			.FirstOrDefault(x => x.ChannelId == (byte)request.Channel
								&& x.NotificationIdentifier == request.Identifier);

		if (settingToUpdate is null)
		{
			throw new ValidationException("No such recipient setting with this identifier and channel id");
		}

		if (settingToUpdate.IsEnabled == request.IsEnabled)
		{
			throw new ValidationException($"Recipient setting already {(request.IsEnabled ? "enabled" : "disabled")}");
		}

		settingToUpdate.IsEnabled = request.IsEnabled;

		await recipientSettingsRepository.SetRange(settingsToUpdate: new List<RecipientSetting> { settingToUpdate });

		return Unit.Value;
	}
}
