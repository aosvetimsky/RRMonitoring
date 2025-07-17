using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("user");

		builder.Property(x => x.TelegramLogin)
			.HasMaxLength(50);

		builder.Property(x => x.UnconfirmedEmail)
			.HasMaxLength(256);

		builder.Property(x => x.PhoneNumber)
			.HasMaxLength(20);

		builder.Property(x => x.FirstName)
			.HasMaxLength(30)
			.IsRequired();

		builder.Property(x => x.LastName)
			.HasMaxLength(50);

		builder.Property(x => x.MiddleName)
			.HasMaxLength(50);

		builder.Property(x => x.SerialNumber)
			.ValueGeneratedOnAdd();

		builder.HasOne(x => x.Status)
			.WithMany()
			.HasForeignKey(x => x.StatusId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.Type)
			.WithMany()
			.HasForeignKey(x => x.TypeId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.ExternalSource)
			.WithMany()
			.HasForeignKey(x => x.ExternalSourceId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(x => x.BlockedUser)
			.WithMany()
			.HasForeignKey(x => x.BlockedBy);

		builder.HasIndex(x => x.ExternalId)
			.IsUnique();
	}
}
