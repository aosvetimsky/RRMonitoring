using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Notification.Infrastructure.Migrations;

public partial class AddPushRegisteredDevice : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "push_registered_device",
			columns: table => new
			{
				recipient_id = table.Column<Guid>(type: "uuid", nullable: false),
				device_id = table.Column<string>(type: "text", nullable: false),
				token = table.Column<string>(type: "text", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_push_registered_device", x => new { x.recipient_id, x.device_id });
			});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "push_registered_device");
	}
}
