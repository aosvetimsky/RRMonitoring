using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

internal class TenantUserConfiguration : IEntityTypeConfiguration<TenantUser>
{
	public void Configure(EntityTypeBuilder<TenantUser> builder)
	{
		builder.ToTable("tenant_user");

		builder.HasKey(x => new { x.TenantId, x.UserId });

		builder.HasOne(x => x.Tenant)
			.WithMany()
			.HasForeignKey(x => x.TenantId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.User)
			.WithMany(x => x.UserTenants)
			.HasForeignKey(x => x.UserId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
