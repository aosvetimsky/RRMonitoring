using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

public class UsedUserPasswordConfiguration : IEntityTypeConfiguration<UsedUserPassword>
{
	public void Configure(EntityTypeBuilder<UsedUserPassword> builder)
	{
		builder.ToTable("used_user_password");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.CreatedDate)
			.HasDefaultValueSql("now()");

		builder.HasOne(x => x.User)
			.WithMany(x => x.UsedPasswords)
			.HasForeignKey(x => x.UserId);

		builder.HasIndex(x => x.UserId);
	}
}
