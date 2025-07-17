using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

public class PermissionGrantPermissionConfiguration : IEntityTypeConfiguration<PermissionGrantPermission>
{
	public void Configure(EntityTypeBuilder<PermissionGrantPermission> builder)
	{
		builder.ToTable("permission_grant_permission");

		builder.HasKey(x => new { x.PermissionGrantId, x.PermissionId });

		builder
			.HasOne(x => x.PermissionGrant)
			.WithMany(x => x.GrantedPermissions)
			.HasForeignKey(x => x.PermissionGrantId);

		builder
			.HasOne(x => x.Permission)
			.WithMany()
			.HasForeignKey(x => x.PermissionId);
	}
}
