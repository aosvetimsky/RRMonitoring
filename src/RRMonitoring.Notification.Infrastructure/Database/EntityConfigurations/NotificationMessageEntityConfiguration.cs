using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Infrastructure.Database.EntityConfigurations;

public class NotificationMessageEntityConfiguration : IEntityTypeConfiguration<NotificationMessage>
{
	public void Configure(EntityTypeBuilder<NotificationMessage> builder)
	{
		builder.ToTable("notification_message");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.RecipientAddress)
			.IsRequired();

		builder.HasOne(x => x.Channel)
			.WithMany()
			.HasForeignKey(x => x.ChannelId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.Notification)
			.WithMany()
			.HasForeignKey(x => x.NotificationId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasMany(x => x.NotificationMessageHistory)
			.WithOne(x => x.NotificationMessage)
			.HasForeignKey(x => x.NotificationMessageId);

		builder.HasIndex(x => x.ExternalMessageId);
	}
}
