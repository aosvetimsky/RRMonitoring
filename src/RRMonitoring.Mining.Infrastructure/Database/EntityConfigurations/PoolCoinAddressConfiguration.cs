using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Mining.Domain.Entities;

namespace RRMonitoring.Mining.Infrastructure.Database.EntityConfigurations;

public class PoolCoinAddressConfiguration : IEntityTypeConfiguration<PoolCoinAddress>
{
	public void Configure(EntityTypeBuilder<PoolCoinAddress> builder)
	{
		builder.ToTable("pool_coin_address");

		builder.HasKey(x => new { x.PoolId, x.CoinId });

		builder
			.HasOne(x => x.Coin)
			.WithMany()
			.HasForeignKey(x => x.CoinId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
