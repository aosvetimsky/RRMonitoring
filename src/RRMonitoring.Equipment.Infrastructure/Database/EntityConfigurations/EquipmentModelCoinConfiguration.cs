using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Infrastructure.Database.EntityConfigurations;

public class EquipmentModelCoinConfiguration : IEntityTypeConfiguration<EquipmentModelCoin>
{
	public void Configure(EntityTypeBuilder<EquipmentModelCoin> builder)
	{
		builder.ToTable("equipment_model_coin");

		builder.HasKey(x => new { x.EquipmentModelId, x.CoinId });

		builder
			.HasOne(x => x.EquipmentModel)
			.WithMany(x => x.EquipmentModelCoins)
			.HasForeignKey(x => x.EquipmentModelId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}