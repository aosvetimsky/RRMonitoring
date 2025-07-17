using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Notification.Infrastructure.Migrations;

public partial class AddExternalSystemFieldsToHotificationHistory : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<string>(
			name: "external_message_id",
			table: "notification_history",
			type: "text",
			nullable: true);

		migrationBuilder.AddColumn<string>(
			name: "external_system_status",
			table: "notification_history",
			type: "text",
			nullable: true);

		migrationBuilder.InsertData(
			table: "status",
			columns: new[] { "id", "name" },
			values: new object[,]
			{
				{ (byte)1, "queued" },
				{ (byte)2, "sent" },
				{ (byte)3, "failed" }
			});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DeleteData(
			table: "status",
			keyColumn: "id",
			keyValue: (byte)1);

		migrationBuilder.DeleteData(
			table: "status",
			keyColumn: "id",
			keyValue: (byte)2);

		migrationBuilder.DeleteData(
			table: "status",
			keyColumn: "id",
			keyValue: (byte)3);

		migrationBuilder.DropColumn(
			name: "external_message_id",
			table: "notification_history");

		migrationBuilder.DropColumn(
			name: "external_system_status",
			table: "notification_history");
	}
}
