using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

public class UserTypeConfiguration : IEntityTypeConfiguration<UserType>
{
	public void Configure(EntityTypeBuilder<UserType> builder)
	{
		builder.ToTable("user_type");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(x => x.Code)
			.IsRequired()
			.HasMaxLength(50);

		builder.HasIndex(x => x.Code)
			.IsUnique();
	}
}
