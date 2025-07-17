using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddExternalIdToUser : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<string>(
			name: "external_id",
			table: "user",
			type: "text",
			nullable: true);

		migrationBuilder.CreateIndex(
			name: "ix_user_external_id",
			table: "user",
			column: "external_id",
			unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "ix_user_external_id",
			table: "user");

		migrationBuilder.DropColumn(
			name: "external_id",
			table: "user");
	}
}
