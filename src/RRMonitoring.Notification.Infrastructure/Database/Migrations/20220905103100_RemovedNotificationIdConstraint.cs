using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Notification.Infrastructure.Migrations;

public partial class RemovedNotificationIdConstraint : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<Guid>(
			name: "notification_id",
			table: "notification_history",
			nullable: true);

		migrationBuilder.AlterColumn<Guid>(
			name: "notification_id",
			table: "notification_push_history",
			nullable: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
	}
}
