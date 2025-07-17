using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RRMonitoring.Mining.Infrastructure.Database;

internal class MiningContextFactory : IDesignTimeDbContextFactory<MiningContext>
{
	public MiningContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<MiningContext>();
		optionsBuilder.UseNpgsql(
			"User ID=postgres;Password=password;Host=localhost;Port=5432;Database=monitoring_mining;");

		return new MiningContext(optionsBuilder.Options);
	}
}
