using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RRMonitoring.Notification.Infrastructure.Database.EntityConfigurations;

public class NotificationEntityConfiguration : IEntityTypeConfiguration<Domain.Entities.Notification>
{
	public void Configure(EntityTypeBuilder<Domain.Entities.Notification> builder)
	{
		builder.ToTable("notification");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Identifier)
			.IsRequired()
			.HasMaxLength(250);

		builder.Property(x => x.Description)
			.HasMaxLength(250);

		builder.HasOne(x => x.Group)
			.WithMany(x => x.Notifications)
			.HasForeignKey(x => x.GroupId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasIndex(x => x.Identifier)
			.IsUnique();
	}
}
