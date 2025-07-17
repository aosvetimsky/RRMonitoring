using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Infrastructure.Database.Repositories;

public class RecipientSettingsRepository(NotificationContext context)
	: IRecipientSettingsRepository
{
	private readonly DbSet<RecipientSetting> _recipientSettingsDbSet = context.Set<RecipientSetting>();

	public async Task<IList<RecipientSetting>> GetByRecipientId(string id)
	{
		return await _recipientSettingsDbSet
			.Include(x => x.Notification)
			.Where(x => x.RecipientId == id)
			.AsNoTracking()
			.ToListAsync();
	}

	public async Task<IList<RecipientSetting>> GetByRecipientIds(IList<string> recipientIds)
	{
		return await _recipientSettingsDbSet
			.Where(x => recipientIds.Contains(x.RecipientId))
			.AsNoTracking()
			.ToListAsync();
	}

	public async Task<bool> IsNotificationEnabledForRecipient(string recipientId, string notificationIdentifier)
	{
		var recipientNotificationSetting = await _recipientSettingsDbSet.FirstOrDefaultAsync(x =>
			x.RecipientId == recipientId && x.NotificationIdentifier == notificationIdentifier);

		return recipientNotificationSetting?.IsEnabled ?? true;
	}

	public async Task SetRange(
		IList<RecipientSetting> settingsToAdd = null,
		IList<RecipientSetting> settingsToUpdate = null,
		IList<RecipientSetting> settingsToRemove = null)
	{
		await using var transaction = await context.Database.BeginTransactionAsync();

		try
		{
			if (settingsToAdd?.Any() == true)
			{
				await _recipientSettingsDbSet.AddRangeAsync(settingsToAdd);
			}

			if (settingsToUpdate?.Any() == true)
			{
				_recipientSettingsDbSet.UpdateRange(settingsToUpdate);
			}

			if (settingsToRemove?.Any() == true)
			{
				_recipientSettingsDbSet.RemoveRange(settingsToRemove);
			}

			await context.SaveChangesAsync();

			await transaction.CommitAsync();
		}
		catch (Exception)
		{
			await transaction.RollbackAsync();
			throw;
		}
	}
}
