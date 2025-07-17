using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RRMonitoring.Notification.Infrastructure.Database;

public class NotificationContextFactory : IDesignTimeDbContextFactory<NotificationContext>
{
	public NotificationContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<NotificationContext>();
		optionsBuilder.UseNpgsql(
			"User ID=postgres;Password=password;Host=localhost;Port=5432;Database=notificationDb;");

		return new NotificationContext(optionsBuilder.Options);
	}
}
