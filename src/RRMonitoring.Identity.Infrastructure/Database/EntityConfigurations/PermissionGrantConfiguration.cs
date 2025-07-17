

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

internal sealed class PermissionGrantConfiguration : IEntityTypeConfiguration<PermissionGrant>
{
	public void Configure(EntityTypeBuilder<PermissionGrant> builder)
	{
		builder.ToTable("permission_grant");

		builder.HasKey(x => x.Id);

		builder.HasOne(x => x.SourceUser)
			.WithMany()
			.HasForeignKey(x => x.SourceUserId);

		builder.HasOne(x => x.DestinationUser)
			.WithMany()
			.HasForeignKey(x => x.DestinationUserId);

		builder.Property(x => x.Reason)
			.HasMaxLength(256);
	}
}
