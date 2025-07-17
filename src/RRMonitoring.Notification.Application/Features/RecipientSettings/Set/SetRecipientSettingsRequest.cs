using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Features.RecipientSettings.Set;

public class SetRecipientSettingsRequest : IRequest<Unit>
{
	public IList<SetRecipientSettings> SetRecipientSettings { get; set; }
}

public class SetRecipientSettings
{
	public string RecipientId { get; set; }
	public Channels Channel { get; set; }
	public string NotificationIdentifier { get; set; }
	public bool? IsEnabled { get; set; }
}

public class SetRecipientSettingsHandler(
	IRecipientSettingsRepository recipientSettingsRepository,
	IMapper mapper)
	: IRequestHandler<SetRecipientSettingsRequest>
{
	public async Task<Unit> Handle(SetRecipientSettingsRequest request, CancellationToken cancellationToken)
	{
		var recipientIds = request.SetRecipientSettings
			.Select(x => x.RecipientId)
			.Distinct()
			.ToList();

		var existingSettings = await recipientSettingsRepository.GetByRecipientIds(recipientIds);

		var newSettings = new List<RecipientSetting>();
		var settingsToUpdate = new List<RecipientSetting>();

		foreach (var item in request.SetRecipientSettings)
		{
			var existingSetting = existingSettings.FirstOrDefault(x => x.RecipientId == item.RecipientId
									&& x.ChannelId == (byte)item.Channel
									&& x.NotificationIdentifier == item.NotificationIdentifier);

			if (existingSetting == null)
			{
				var recipientSettings = mapper.Map<RecipientSetting>(item);
				recipientSettings.IsEnabled = item.IsEnabled ?? true;

				newSettings.Add(recipientSettings);
			}
			else
			{
				if (item.IsEnabled.HasValue)
				{
					existingSetting.IsEnabled = item.IsEnabled.Value;
				}

				settingsToUpdate.Add(existingSetting);
			}
		}

		var settingsToRemove = existingSettings.Except(settingsToUpdate).ToList();

		await recipientSettingsRepository.SetRange(newSettings, settingsToUpdate, settingsToRemove);

		return Unit.Value;
	}
}
