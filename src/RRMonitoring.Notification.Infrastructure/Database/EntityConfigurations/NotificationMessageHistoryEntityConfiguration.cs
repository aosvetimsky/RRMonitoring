using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Infrastructure.Database.EntityConfigurations;

public class NotificationMessageHistoryEntityConfiguration : IEntityTypeConfiguration<NotificationMessageHistory>
{
	public void Configure(EntityTypeBuilder<NotificationMessageHistory> builder)
	{
		builder.ToTable("notification_message_history");

		builder.HasKey(x => x.Id);

		builder.HasOne(x => x.NotificationMessage)
			.WithMany(x => x.NotificationMessageHistory)
			.HasForeignKey(x => x.NotificationMessageId)
			.IsRequired();

		builder.HasOne(x => x.Status)
			.WithMany()
			.HasForeignKey(x => x.StatusId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
