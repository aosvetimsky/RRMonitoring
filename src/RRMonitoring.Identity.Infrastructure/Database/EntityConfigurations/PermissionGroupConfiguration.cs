using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

internal class PermissionGroupConfiguration : IEntityTypeConfiguration<PermissionGroup>
{
	public void Configure(EntityTypeBuilder<PermissionGroup> builder)
	{
		builder.ToTable("permission_group");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.HasMaxLength(250);

		builder.HasMany(x => x.Permissions)
			.WithOne(x => x.Group)
			.HasForeignKey(x => x.GroupId);

		builder.HasIndex(x => x.Name)
			.IsUnique();
	}
}
