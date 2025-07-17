using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nomium.Core.Data.EntityFrameworkCore.Extensions;
using RRMonitoring.Notification.Infrastructure.Database.EntityConfigurations;

namespace RRMonitoring.Notification.Infrastructure.Database;

public class NotificationContext(DbContextOptions<NotificationContext> options)
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
		modelBuilder.ApplyConfiguration(new ChannelEntityConfiguration());
		modelBuilder.ApplyConfiguration(new NotificationEntityConfiguration());
		modelBuilder.ApplyConfiguration(new NotificationGroupEntityConfiguration());
		modelBuilder.ApplyConfiguration(new NotificationMessageEntityConfiguration());
		modelBuilder.ApplyConfiguration(new NotificationMessageHistoryEntityConfiguration());
		modelBuilder.ApplyConfiguration(new RecipientSettingEntityConfiguration());
		modelBuilder.ApplyConfiguration(new StatusEntityConfiguration());
		modelBuilder.ApplyConfiguration(new TemplateEntityConfiguration());
		modelBuilder.ApplyConfiguration(new PushRegisteredDeviceEntityConfiguration());
	}
}
