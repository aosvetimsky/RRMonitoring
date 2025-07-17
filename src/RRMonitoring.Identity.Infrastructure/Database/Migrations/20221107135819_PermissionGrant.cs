using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class PermissionGrant : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "permission_grant",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				source_user_id = table.Column<Guid>(type: "uuid", nullable: false),
				destination_user_id = table.Column<Guid>(type: "uuid", nullable: false),
				start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				reason = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
				created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				created_by = table.Column<Guid>(type: "uuid", nullable: false),
				updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
				updated_by = table.Column<Guid>(type: "uuid", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_permission_grant", x => x.id);
				table.ForeignKey(
					name: "fk_permission_grant_user_destination_user_id",
					column: x => x.destination_user_id,
					principalTable: "user",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "fk_permission_grant_user_source_user_id",
					column: x => x.source_user_id,
					principalTable: "user",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateTable(
			name: "permission_grant_permission",
			columns: table => new
			{
				permission_grant_id = table.Column<Guid>(type: "uuid", nullable: false),
				permission_id = table.Column<Guid>(type: "uuid", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_permission_grant_permission", x => new { x.permission_grant_id, x.permission_id });
				table.ForeignKey(
					name: "fk_permission_grant_permission_permission_grant_permission_gra",
					column: x => x.permission_grant_id,
					principalTable: "permission_grant",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "fk_permission_grant_permission_permission_permission_id",
					column: x => x.permission_id,
					principalTable: "permission",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "ix_permission_grant_destination_user_id",
			table: "permission_grant",
			column: "destination_user_id");

		migrationBuilder.CreateIndex(
			name: "ix_permission_grant_source_user_id",
			table: "permission_grant",
			column: "source_user_id");

		migrationBuilder.CreateIndex(
			name: "ix_permission_grant_permission_permission_id",
			table: "permission_grant_permission",
			column: "permission_id");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "permission_grant_permission");

		migrationBuilder.DropTable(
			name: "permission_grant");
	}
}
