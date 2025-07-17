using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Infrastructure.Database.EntityConfigurations;

public class EquipmentModelConfiguration : IEntityTypeConfiguration<EquipmentModel>
{
	public void Configure(EntityTypeBuilder<EquipmentModel> builder)
	{
		builder.ToTable("equipment_model");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired();

		builder
			.HasOne(x => x.HashrateUnit)
			.WithMany()
			.HasForeignKey(x => x.HashrateUnitId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasMany(x => x.Equipment)
			.WithOne(x => x.Model)
			.HasForeignKey(x => x.ModelId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
