using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class FixNameForOpenApiKeyIpWhiteList : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_api_key_permission_api_key_api_key_temp_id",
			table: "api_key_permission");

		migrationBuilder.RenameColumn(
			name: "ip_whitelist",
			table: "api_key",
			newName: "ip_white_list");

		migrationBuilder.AddForeignKey(
			name: "fk_api_key_permission_api_keys_api_key_temp_id",
			table: "api_key_permission",
			column: "key",
			principalTable: "api_key",
			principalColumn: "key",
			onDelete: ReferentialAction.Cascade);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_api_key_permission_api_keys_api_key_temp_id",
			table: "api_key_permission");

		migrationBuilder.RenameColumn(
			name: "ip_white_list",
			table: "api_key",
			newName: "ip_whitelist");

		migrationBuilder.AddForeignKey(
			name: "fk_api_key_permission_api_key_api_key_temp_id",
			table: "api_key_permission",
			column: "key",
			principalTable: "api_key",
			principalColumn: "key",
			onDelete: ReferentialAction.Cascade);
	}
}
