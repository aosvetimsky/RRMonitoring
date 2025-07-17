using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

internal class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
	public void Configure(EntityTypeBuilder<Tenant> builder)
	{
		builder.ToTable("tenant");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.HasMaxLength(50);

		builder.Property(x => x.Code)
			.HasMaxLength(50);

		builder.HasIndex(x => x.Name);

		builder.HasIndex(x => x.Code)
			.IsUnique();
	}
}
