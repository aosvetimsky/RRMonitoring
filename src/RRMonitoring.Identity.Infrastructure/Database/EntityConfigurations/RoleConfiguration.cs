using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.ToTable("role");

		builder.Property(x => x.Code)
			.HasMaxLength(50);

		builder
			.HasIndex(x => x.Code)
			.IsUnique()
			.HasFilter("tenant_id IS NULL");

		builder
			.HasIndex(x => new { x.Code, x.TenantId })
			.IsUnique();

		builder
			.HasIndex(x => x.NormalizedName)
			.IsUnique()
			.HasFilter("tenant_id IS NULL");

		builder
			.HasIndex(x => new { x.NormalizedName, x.TenantId })
			.IsUnique();
	}
}
