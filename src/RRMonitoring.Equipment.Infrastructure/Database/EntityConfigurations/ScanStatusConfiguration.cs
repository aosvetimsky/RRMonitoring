using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Infrastructure.Database.EntityConfigurations;

public class ScanStatusConfiguration : IEntityTypeConfiguration<ScanStatus>
{
	public void Configure(EntityTypeBuilder<ScanStatus> builder)
	{
		builder.ToTable("scan_status");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired();
	}
}
