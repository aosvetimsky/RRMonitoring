using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Infrastructure.Database.EntityConfigurations;

public class NotificationGroupEntityConfiguration : IEntityTypeConfiguration<NotificationGroup>
{
	public void Configure(EntityTypeBuilder<NotificationGroup> builder)
	{
		builder.ToTable("notification_group");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(250);
	}
}
