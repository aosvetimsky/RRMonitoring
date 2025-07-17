using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Infrastructure.Database.EntityConfigurations;

public class ScanConfiguration : IEntityTypeConfiguration<Scan>
{
	public void Configure(EntityTypeBuilder<Scan> builder)
	{
		builder.ToTable("scan");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired();

		builder
			.Property(x => x.IpRangeDefinition)
			.IsRequired();

		builder
			.HasOne(x => x.Status)
			.WithMany()
			.HasForeignKey(x => x.StatusId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasMany(x => x.ScanResults)
			.WithOne(x => x.Scan)
			.HasForeignKey(x => x.ScanId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
