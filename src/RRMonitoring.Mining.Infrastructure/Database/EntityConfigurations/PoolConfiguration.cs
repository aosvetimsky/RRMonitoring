using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Mining.Domain.Entities;

namespace RRMonitoring.Mining.Infrastructure.Database.EntityConfigurations;

public class PoolConfiguration : IEntityTypeConfiguration<Pool>
{
	public void Configure(EntityTypeBuilder<Pool> builder)
	{
		builder.ToTable("pool");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired();

		builder
			.Property(x => x.ExternalId)
			.IsRequired();

		builder.HasIndex(x => x.ExternalId)
			.IsUnique();

		builder
			.HasMany(x => x.CoinAddresses)
			.WithOne(x => x.Pool)
			.HasForeignKey(x => x.PoolId)
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.HasMany(x => x.Workers)
			.WithOne(x => x.Pool)
			.HasForeignKey(x => x.PoolId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
