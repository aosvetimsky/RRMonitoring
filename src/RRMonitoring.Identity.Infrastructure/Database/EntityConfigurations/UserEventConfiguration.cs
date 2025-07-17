using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

public class UserEventConfiguration : IEntityTypeConfiguration<UserEvent>
{
	public void Configure(EntityTypeBuilder<UserEvent> builder)
	{
		builder.ToTable("user_events");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.EventKind)
			.HasConversion<string>();

		builder.Property(x => x.EventDate)
			.HasDefaultValueSql("now()");

		builder.HasOne(x => x.User)
			.WithMany(x => x.UserEvents)
			.HasForeignKey(x => x.UserId);
	}
}
