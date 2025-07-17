using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class UpdateObserverLinkPermissionLinks : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_api_key_permission_api_key_api_key_key",
			table: "api_key_permission");

		migrationBuilder.AddForeignKey(
			name: "fk_api_key_permission_api_key_api_key_temp_id",
			table: "api_key_permission",
			column: "key",
			principalTable: "api_key",
			principalColumn: "key",
			onDelete: ReferentialAction.Cascade);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_api_key_permission_api_key_api_key_temp_id",
			table: "api_key_permission");

		migrationBuilder.AddForeignKey(
			name: "fk_api_key_permission_api_key_api_key_key",
			table: "api_key_permission",
			column: "key",
			principalTable: "api_key",
			principalColumn: "key",
			onDelete: ReferentialAction.Cascade);
	}
}
