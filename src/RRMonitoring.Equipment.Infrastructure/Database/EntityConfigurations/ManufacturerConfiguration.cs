using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Infrastructure.Database.EntityConfigurations;

public class ManufacturerConfiguration: IEntityTypeConfiguration<Manufacturer>
{
	public void Configure(EntityTypeBuilder<Manufacturer> builder)
	{
		builder.ToTable("manufacturer");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired();

		builder
			.HasMany(x => x.EquipmentModels)
			.WithOne(x => x.Manufacturer)
			.HasForeignKey(x => x.ManufacturerId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
