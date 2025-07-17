using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Notification.Infrastructure.Migrations;

public partial class NotificationHistoryErrorText : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<string>(
			name: "error_text",
			table: "notification_history",
			type: "text",
			nullable: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "error_text",
			table: "notification_history");
	}
}
