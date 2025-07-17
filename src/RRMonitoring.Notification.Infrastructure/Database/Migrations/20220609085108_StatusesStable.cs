using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Notification.Infrastructure.Migrations;

public partial class StatusesStable : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.UpdateData(
			table: "status",
			keyColumn: "id",
			keyValue: (byte)2,
			column: "name",
			value: "delivered");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.UpdateData(
			table: "status",
			keyColumn: "id",
			keyValue: (byte)2,
			column: "name",
			value: "sent");
	}
}
