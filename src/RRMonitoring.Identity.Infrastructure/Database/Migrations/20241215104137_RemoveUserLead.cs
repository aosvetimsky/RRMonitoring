using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class RemoveUserLead : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_user_user_lead_id",
			table: "user");

		migrationBuilder.DropIndex(
			name: "ix_user_lead_id",
			table: "user");

		migrationBuilder.DropColumn(
			name: "lead_id",
			table: "user");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<Guid>(
			name: "lead_id",
			table: "user",
			type: "uuid",
			nullable: true);

		migrationBuilder.CreateIndex(
			name: "ix_user_lead_id",
			table: "user",
			column: "lead_id");

		migrationBuilder.AddForeignKey(
			name: "fk_user_user_lead_id",
			table: "user",
			column: "lead_id",
			principalTable: "user",
			principalColumn: "id");
	}
}
