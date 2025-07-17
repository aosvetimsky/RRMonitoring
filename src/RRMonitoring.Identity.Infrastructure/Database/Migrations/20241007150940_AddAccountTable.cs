using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddAccountTable : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "account",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
				description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
				user_id = table.Column<Guid>(type: "uuid", nullable: false),
				parent_id = table.Column<Guid>(type: "uuid", nullable: true),
				created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				created_by = table.Column<Guid>(type: "uuid", nullable: false),
				updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
				updated_by = table.Column<Guid>(type: "uuid", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_account", x => x.id);
				table.ForeignKey(
					name: "fk_account_account_parent_id",
					column: x => x.parent_id,
					principalTable: "account",
					principalColumn: "id",
					onDelete: ReferentialAction.Restrict);
				table.ForeignKey(
					name: "fk_account_user_user_id",
					column: x => x.user_id,
					principalTable: "user",
					principalColumn: "id",
					onDelete: ReferentialAction.Restrict);
			});

		migrationBuilder.CreateIndex(
			name: "ix_account_name",
			table: "account",
			column: "name",
			unique: true);

		migrationBuilder.CreateIndex(
			name: "ix_account_parent_id",
			table: "account",
			column: "parent_id");

		migrationBuilder.CreateIndex(
			name: "ix_account_user_id_parent_id",
			table: "account",
			columns: new[] { "user_id", "parent_id" },
			unique: true,
			filter: "parent_id IS NULL");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "account");
	}
}
