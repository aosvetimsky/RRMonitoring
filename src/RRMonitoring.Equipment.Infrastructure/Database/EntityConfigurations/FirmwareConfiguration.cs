using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Infrastructure.Database.EntityConfigurations;

public class FirmwareConfiguration : IEntityTypeConfiguration<Firmware>
{
	public void Configure(EntityTypeBuilder<Firmware> builder)
	{
		builder.ToTable("firmware");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired();

		builder
			.Property(x => x.Version)
			.IsRequired();

		builder
			.Property(x => x.OriginFileName)
			.IsRequired();
	}
}
