using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Notification.Infrastructure.Migrations;

public partial class AddNotificationPushHistory : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "notification_push_history",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				recipient_id = table.Column<string>(type: "text", nullable: false),
				external_message_id = table.Column<string>(type: "text", nullable: false),
				notification_body = table.Column<string>(type: "text", nullable: true),
				is_read = table.Column<bool>(type: "boolean", nullable: false),
				notification_id = table.Column<Guid>(type: "uuid", nullable: false),
				created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				created_by = table.Column<Guid>(type: "uuid", nullable: false),
				updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
				updated_by = table.Column<Guid>(type: "uuid", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_notification_push_history", x => x.id);
				table.ForeignKey(
					name: "fk_notification_push_history_notification_notification_id",
					column: x => x.notification_id,
					principalTable: "notification",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "ix_notification_push_history_notification_id",
			table: "notification_push_history",
			column: "notification_id");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "notification_push_history");
	}
}
