using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nomium.Core.Data.EntityFrameworkCore.Extensions;

namespace RRMonitoring.Colocation.Infrastructure.Database;

public sealed class ColocationContext(DbContextOptions<ColocationContext> options)
	: DbContext(options)
{
	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		this.SetAuditableEntitiesCreateUpdateDates();

		return base.SaveChangesAsync(cancellationToken);
	}

	public override int SaveChanges()
	{
		this.SetAuditableEntitiesCreateUpdateDates();

		return base.SaveChanges();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSnakeCaseNamingConvention();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ColocationContext).Assembly);
	}
}
