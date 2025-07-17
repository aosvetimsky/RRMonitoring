using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

internal class ScopePermissionConfiguration : IEntityTypeConfiguration<ScopePermission>
{
	public void Configure(EntityTypeBuilder<ScopePermission> builder)
	{
		builder.ToTable("scope_permission");

		builder.HasKey(x => new { x.ScopeId, x.PermissionId });

		builder
			.HasOne(x => x.Permission)
			.WithMany()
			.HasForeignKey(x => x.PermissionId);

		builder
			.HasOne(x => x.Scope)
			.WithMany()
			.HasForeignKey(x => x.ScopeId);
	}
}
