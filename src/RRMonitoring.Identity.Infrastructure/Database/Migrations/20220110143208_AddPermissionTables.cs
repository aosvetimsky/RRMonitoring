using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddPermissionTables : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "permission_group",
			columns: table => new
			{
				id = table.Column<int>(type: "integer", nullable: false)
					.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
				name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_permission_group", x => x.id);
			});

		migrationBuilder.CreateTable(
			name: "permission",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
				display_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
				group_id = table.Column<int>(type: "integer", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_permission", x => x.id);
				table.ForeignKey(
					name: "fk_permission_permission_group_permission_group_id",
					column: x => x.group_id,
					principalTable: "permission_group",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateTable(
			name: "role_permission",
			columns: table => new
			{
				role_id = table.Column<Guid>(type: "uuid", nullable: false),
				permission_id = table.Column<Guid>(type: "uuid", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_role_permission", x => new { x.role_id, x.permission_id });
				table.ForeignKey(
					name: "fk_role_permission_permission_permission_id",
					column: x => x.permission_id,
					principalTable: "permission",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "fk_role_permission_role_role_id",
					column: x => x.role_id,
					principalTable: "role",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateTable(
			name: "scope_permission",
			columns: table => new
			{
				scope_id = table.Column<int>(type: "integer", nullable: false),
				permission_id = table.Column<Guid>(type: "uuid", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_scope_permission", x => new { x.scope_id, x.permission_id });
				table.ForeignKey(
					name: "fk_scope_permission_api_scope_scope_id",
					column: x => x.scope_id,
					principalTable: "api_scope",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "fk_scope_permission_permission_permission_id",
					column: x => x.permission_id,
					principalTable: "permission",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "ix_permission_group_id",
			table: "permission",
			column: "group_id");

		migrationBuilder.CreateIndex(
			name: "ix_role_permission_permission_id",
			table: "role_permission",
			column: "permission_id");

		migrationBuilder.CreateIndex(
			name: "ix_scope_permission_permission_id",
			table: "scope_permission",
			column: "permission_id");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "role_permission");

		migrationBuilder.DropTable(
			name: "scope_permission");

		migrationBuilder.DropTable(
			name: "permission");

		migrationBuilder.DropTable(
			name: "permission_group");
	}
}
