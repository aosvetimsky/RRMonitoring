using System.Collections.Generic;
using System.Threading.Tasks;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Domain.Contracts;

public interface IRecipientSettingsRepository
{
	Task<IList<RecipientSetting>> GetByRecipientId(string id);

	Task<IList<RecipientSetting>> GetByRecipientIds(IList<string> recipientIds);

	Task SetRange(
		IList<RecipientSetting> settingsToAdd = null,
		IList<RecipientSetting> settingsToUpdate = null,
		IList<RecipientSetting> settingsToRemove = null);

	Task<bool> IsNotificationEnabledForRecipient(string recipientId, string notificationIdentifier);
}
