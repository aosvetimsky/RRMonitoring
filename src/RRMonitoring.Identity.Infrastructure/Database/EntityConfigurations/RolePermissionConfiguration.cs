using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

internal class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
	public void Configure(EntityTypeBuilder<RolePermission> builder)
	{
		builder.ToTable("role_permission");

		builder.HasKey(x => new { x.RoleId, x.PermissionId });

		builder
			.HasOne(x => x.Role)
			.WithMany(x => x.RolePermissions)
			.HasForeignKey(x => x.RoleId);

		builder
			.HasOne(x => x.Permission)
			.WithMany(x => x.RolePermissions)
			.HasForeignKey(x => x.PermissionId);
	}
}
