using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Notification.Infrastructure.Migrations;

public partial class NotificationChannelIsUnique : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "ix_template_channel_id",
			table: "template");

		migrationBuilder.AddColumn<string>(
			name: "subject",
			table: "template",
			type: "text",
			nullable: true);

		migrationBuilder.InsertData(
			table: "channel",
			columns: new[] { "id", "name" },
			values: new object[,]
			{
				{ (byte)1, "Email" },
				{ (byte)2, "Push" },
				{ (byte)3, "Sms" }
			});

		migrationBuilder.CreateIndex(
			name: "ix_channel_notification",
			table: "template",
			columns: new[] { "channel_id", "notification_id" },
			unique: true);

		migrationBuilder.CreateIndex(
			name: "ix_notification_identifier",
			table: "notification",
			column: "identifier",
			unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "ix_channel_notification",
			table: "template");

		migrationBuilder.DropIndex(
			name: "ix_notification_identifier",
			table: "notification");

		migrationBuilder.DeleteData(
			table: "channel",
			keyColumn: "id",
			keyValue: (byte)1);

		migrationBuilder.DeleteData(
			table: "channel",
			keyColumn: "id",
			keyValue: (byte)2);

		migrationBuilder.DeleteData(
			table: "channel",
			keyColumn: "id",
			keyValue: (byte)3);

		migrationBuilder.DropColumn(
			name: "subject",
			table: "template");

		migrationBuilder.CreateIndex(
			name: "ix_template_channel_id",
			table: "template",
			column: "channel_id");
	}
}
