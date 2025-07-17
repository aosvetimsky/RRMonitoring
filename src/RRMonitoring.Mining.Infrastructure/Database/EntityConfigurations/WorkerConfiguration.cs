using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Mining.Domain.Entities;

namespace RRMonitoring.Mining.Infrastructure.Database.EntityConfigurations;

public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
{
	public void Configure(EntityTypeBuilder<Worker> builder)
	{
		builder.ToTable("worker");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired();

		builder
			.Property(x => x.DisplayName)
			.IsRequired();

		builder
			.Property(x => x.ExternalId)
			.IsRequired();

		builder.HasIndex(x => x.ExternalId)
			.IsUnique();

		builder.HasIndex(x => new { x.Name, x.PoolId, x.CoinId })
			.IsUnique();

		builder
			.HasOne(x => x.Coin)
			.WithMany()
			.HasForeignKey(x => x.CoinId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(x => x.Status)
			.WithMany()
			.HasForeignKey(x => x.StatusId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
