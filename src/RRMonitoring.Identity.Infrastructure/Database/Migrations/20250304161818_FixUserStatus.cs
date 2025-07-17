using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class FixUserStatus : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.UpdateData(
			table: "user_status",
			keyColumn: "id",
			keyValue: (byte)3,
			column: "name",
			value: "Заблокирован");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.UpdateData(
			table: "user_status",
			keyColumn: "id",
			keyValue: (byte)3,
			column: "name",
			value: "Заблокированный");
	}
}
