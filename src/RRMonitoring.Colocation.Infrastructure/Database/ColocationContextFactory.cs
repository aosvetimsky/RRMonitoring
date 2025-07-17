using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RRMonitoring.Colocation.Infrastructure.Database;

internal class ColocationContextFactory : IDesignTimeDbContextFactory<ColocationContext>
{
	public ColocationContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<ColocationContext>();
		optionsBuilder.UseNpgsql(
			"User ID=postgres;Password=password;Host=localhost;Port=5432;Database=monitoring_colocation;");

		return new ColocationContext(optionsBuilder.Options);
	}
}
