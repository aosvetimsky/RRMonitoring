using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Infrastructure.Database.EntityConfigurations;

public class FirmwareEquipmentModelConfiguration : IEntityTypeConfiguration<FirmwareEquipmentModel>
{
	public void Configure(EntityTypeBuilder<FirmwareEquipmentModel> builder)
	{
		builder.ToTable("firmware_equipment_model");

		builder.HasKey(x => new { x.FirmwareId, x.EquipmentModelId });

		builder
			.HasOne(x => x.Firmware)
			.WithMany(x => x.FirmwareEquipmentModels)
			.HasForeignKey(x => x.FirmwareId)
			.OnDelete(DeleteBehavior.Cascade);

		builder
			.HasOne(x => x.EquipmentModel)
			.WithMany(x => x.FirmwareEquipmentModels)
			.HasForeignKey(x => x.EquipmentModelId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
