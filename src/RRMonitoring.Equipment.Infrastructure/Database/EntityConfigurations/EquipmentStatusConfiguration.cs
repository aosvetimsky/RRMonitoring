using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Infrastructure.Database.EntityConfigurations;

public class EquipmentStatusConfiguration : IEntityTypeConfiguration<EquipmentStatus>
{
	public void Configure(EntityTypeBuilder<EquipmentStatus> builder)
	{
		builder.ToTable("equipment_status");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired();
	}
}
