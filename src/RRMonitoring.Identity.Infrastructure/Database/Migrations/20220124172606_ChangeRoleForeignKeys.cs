using Microsoft.EntityFrameworkCore.Migrations;

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class ChangeRoleForeignKeys : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_permission_permission_group_permission_group_id",
			table: "permission");

		migrationBuilder.DropForeignKey(
			name: "fk_role_permission_role_role_id",
			table: "role_permission");

		migrationBuilder.DropForeignKey(
			name: "fk_user_role_role_role_id",
			table: "user_role");

		migrationBuilder.AddForeignKey(
			name: "fk_permission_permission_group_group_id",
			table: "permission",
			column: "group_id",
			principalTable: "permission_group",
			principalColumn: "id",
			onDelete: ReferentialAction.Cascade);

		migrationBuilder.AddForeignKey(
			name: "fk_role_permission_roles_role_id",
			table: "role_permission",
			column: "role_id",
			principalTable: "role",
			principalColumn: "id",
			onDelete: ReferentialAction.Cascade);

		migrationBuilder.AddForeignKey(
			name: "fk_user_role_asp_net_roles_role_id",
			table: "user_role",
			column: "role_id",
			principalTable: "role",
			principalColumn: "id",
			onDelete: ReferentialAction.Cascade);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_permission_permission_group_group_id",
			table: "permission");

		migrationBuilder.DropForeignKey(
			name: "fk_role_permission_roles_role_id",
			table: "role_permission");

		migrationBuilder.DropForeignKey(
			name: "fk_user_role_asp_net_roles_role_id",
			table: "user_role");

		migrationBuilder.AddForeignKey(
			name: "fk_permission_permission_group_permission_group_id",
			table: "permission",
			column: "group_id",
			principalTable: "permission_group",
			principalColumn: "id",
			onDelete: ReferentialAction.Cascade);

		migrationBuilder.AddForeignKey(
			name: "fk_role_permission_role_role_id",
			table: "role_permission",
			column: "role_id",
			principalTable: "role",
			principalColumn: "id",
			onDelete: ReferentialAction.Cascade);

		migrationBuilder.AddForeignKey(
			name: "fk_user_role_role_role_id",
			table: "user_role",
			column: "role_id",
			principalTable: "role",
			principalColumn: "id",
			onDelete: ReferentialAction.Cascade);
	}
}
