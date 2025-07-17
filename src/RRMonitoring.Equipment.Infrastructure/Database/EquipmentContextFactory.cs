using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RRMonitoring.Equipment.Infrastructure.Database;

internal class EquipmentContextFactory : IDesignTimeDbContextFactory<EquipmentContext>
{
	public EquipmentContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<EquipmentContext>();
		optionsBuilder.UseNpgsql(
			"User ID=postgres;Password=password;Host=localhost;Port=5432;Database=monitoring_equipment;");

		return new EquipmentContext(optionsBuilder.Options);
	}
}
