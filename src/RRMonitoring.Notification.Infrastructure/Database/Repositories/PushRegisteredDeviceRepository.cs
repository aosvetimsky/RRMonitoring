using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Infrastructure.Database.Repositories;

public sealed class PushRegisteredDeviceRepository(NotificationContext context)
	: IPushRegisteredDeviceRepository
{
	private readonly DbSet<PushRegisteredDevice> _recipientFirebaseTokenDbSet = context.Set<PushRegisteredDevice>();

	public async Task<PushRegisteredDevice> Get(string recipientId, string deviceId)
	{
		return await _recipientFirebaseTokenDbSet
			.SingleOrDefaultAsync(x => x.RecipientId == recipientId && x.DeviceId == deviceId);
	}

	public async Task<IList<PushRegisteredDevice>> GetByRecipientId(string recipientId)
	{
		return await _recipientFirebaseTokenDbSet
			.Where(x => x.RecipientId == recipientId)
			.ToListAsync();
	}

	public async Task Add(PushRegisteredDevice device)
	{
		await _recipientFirebaseTokenDbSet.AddAsync(device);
		await context.SaveChangesAsync();
	}

	public async Task Update(PushRegisteredDevice device)
	{
		_recipientFirebaseTokenDbSet.Update(device);
		await context.SaveChangesAsync();
	}

	public async Task Remove(PushRegisteredDevice device)
	{
		_recipientFirebaseTokenDbSet.Remove(device);
		await context.SaveChangesAsync();
	}

	public async Task RemoveRange(IList<PushRegisteredDevice> devices)
	{
		_recipientFirebaseTokenDbSet.RemoveRange(devices);
		await context.SaveChangesAsync();
	}
}
