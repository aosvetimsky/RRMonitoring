using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddDeletedUserStatus : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.InsertData(
			table: "user_status",
			columns: new[] { "id", "name" },
			values: new object[] { (byte)4, "Удалённый" });
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DeleteData(
			table: "user_status",
			keyColumn: "id",
			keyValue: (byte)4);
	}
}
