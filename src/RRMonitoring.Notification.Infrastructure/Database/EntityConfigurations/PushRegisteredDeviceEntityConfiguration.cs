using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Infrastructure.Database.EntityConfigurations;

public class PushRegisteredDeviceEntityConfiguration : IEntityTypeConfiguration<PushRegisteredDevice>
{
	public void Configure(EntityTypeBuilder<PushRegisteredDevice> builder)
	{
		builder.ToTable("push_registered_device");

		builder.HasKey(x => new { x.RecipientId, x.DeviceId });

		builder.Property(x => x.Token)
			.IsRequired();
	}
}
