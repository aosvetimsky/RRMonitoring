using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Infrastructure.Database.EntityConfigurations;

public class HashrateUnitConfiguration : IEntityTypeConfiguration<HashrateUnit>
{
	public void Configure(EntityTypeBuilder<HashrateUnit> builder)
	{
		builder.ToTable("hashrate_unit");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired();

		builder.HasData(GetDefaultHashrateUnits());
	}

	private static IReadOnlyList<HashrateUnit> GetDefaultHashrateUnits()
	{
		return [
			new HashrateUnit(1, "H/s"),
			new HashrateUnit(2, "K/Sol"),
			new HashrateUnit(3, "MH/s"),
			new HashrateUnit(4, "GH/s"),
			new HashrateUnit(5, "TH/s"),
			new HashrateUnit(6, "PH/s")
			];
	}
}
