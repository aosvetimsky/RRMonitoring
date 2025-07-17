using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Infrastructure.Database.EntityConfigurations;

public class TemplateEntityConfiguration : IEntityTypeConfiguration<Template>
{
	public void Configure(EntityTypeBuilder<Template> builder)
	{
		builder.ToTable("template");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Data)
			.IsRequired();

		builder.HasOne(x => x.Channel)
			.WithMany()
			.HasForeignKey(x => x.ChannelId);

		builder.HasOne(x => x.Notification)
			.WithMany(x => x.Templates)
			.HasForeignKey(x => x.NotificationId);

		builder.HasIndex("ChannelId", "NotificationId")
			.IsUnique()
			.HasDatabaseName("ix_channel_notification");
	}
}
