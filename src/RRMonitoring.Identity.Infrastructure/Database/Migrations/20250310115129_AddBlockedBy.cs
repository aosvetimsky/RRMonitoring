using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddBlockedBy : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<Guid>(
			name: "blocked_by",
			table: "user",
			type: "uuid",
			nullable: true);

		migrationBuilder.CreateIndex(
			name: "ix_user_blocked_by",
			table: "user",
			column: "blocked_by");

		migrationBuilder.AddForeignKey(
			name: "fk_user_user_blocked_user_id",
			table: "user",
			column: "blocked_by",
			principalTable: "user",
			principalColumn: "id");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_user_user_blocked_user_id",
			table: "user");

		migrationBuilder.DropIndex(
			name: "ix_user_blocked_by",
			table: "user");

		migrationBuilder.DropColumn(
			name: "blocked_by",
			table: "user");
	}
}
