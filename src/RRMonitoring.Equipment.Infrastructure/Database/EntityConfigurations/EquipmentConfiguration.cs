using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RRMonitoring.Equipment.Infrastructure.Database.EntityConfigurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Domain.Entities.Equipment>
{
	public void Configure(EntityTypeBuilder<Domain.Entities.Equipment> builder)
	{
		builder.ToTable("equipment");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.SerialNumber)
			.IsRequired();

		builder
			.Property(x => x.MacAddress)
			.IsRequired();

		builder.HasIndex(x => x.IpAddress).IsUnique();

		builder
			.HasOne(x => x.Mode)
			.WithMany()
			.HasForeignKey(x => x.ModeId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(x => x.Status)
			.WithMany()
			.HasForeignKey(x => x.StatusId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasMany(x => x.ScanResults)
			.WithOne(x => x.Equipment)
			.HasForeignKey(x => x.EquipmentId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
