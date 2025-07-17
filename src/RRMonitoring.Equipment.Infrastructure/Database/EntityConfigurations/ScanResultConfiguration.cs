using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Infrastructure.Database.EntityConfigurations;

public class ScanResultConfiguration : IEntityTypeConfiguration<ScanResult>
{
	public void Configure(EntityTypeBuilder<ScanResult> builder)
	{
		builder.ToTable("scan_result");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.IpAddress)
			.IsRequired();

		builder
			.Property(x => x.MacAddress)
			.IsRequired();

		builder
			.Property(x => x.DetectedModel)
			.IsRequired();
	}
}
