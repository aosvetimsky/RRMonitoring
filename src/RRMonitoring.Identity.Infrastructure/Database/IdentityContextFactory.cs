using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RRMonitoring.Identity.Infrastructure.Database;

public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
{
	public IdentityContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
		optionsBuilder.UseNpgsql("User ID=postgres;Password=password;Host=localhost;Port=5432;Database=grotem_identityDb;");

		return new IdentityContext(optionsBuilder.Options);
	}
}
