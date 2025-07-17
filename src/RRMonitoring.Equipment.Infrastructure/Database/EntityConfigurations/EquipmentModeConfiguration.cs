using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Infrastructure.Database.EntityConfigurations;

public class EquipmentModeConfiguration : IEntityTypeConfiguration<EquipmentMode>
{
	public void Configure(EntityTypeBuilder<EquipmentMode> builder)
	{
		builder.ToTable("equipment_mode");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired();
	}
}
