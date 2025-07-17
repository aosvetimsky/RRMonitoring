using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Infrastructure.Database.EntityConfigurations;

public class RecipientSettingEntityConfiguration : IEntityTypeConfiguration<RecipientSetting>
{
	public void Configure(EntityTypeBuilder<RecipientSetting> builder)
	{
		builder.ToTable("recipient_setting");

		builder.HasKey(x => new { x.RecipientId, x.ChannelId, x.NotificationIdentifier });

		builder.Property(x => x.RecipientId)
			.IsRequired();

		builder.HasOne(x => x.Channel)
			.WithMany()
			.HasForeignKey(x => x.ChannelId);

		builder.HasOne(x => x.Notification)
			.WithMany()
			.HasForeignKey(x => x.NotificationIdentifier)
			.HasPrincipalKey(x => x.Identifier);
	}
}
