using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddAccountCompositeIndex : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "ix_account_name",
			table: "account");

		migrationBuilder.CreateIndex(
			name: "ix_account_user_id_name_is_deleted",
			table: "account",
			columns: new[] { "user_id", "name", "is_deleted" },
			unique: true,
			filter: "is_deleted = false");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "ix_account_user_id_name_is_deleted",
			table: "account");

		migrationBuilder.CreateIndex(
			name: "ix_account_name",
			table: "account",
			column: "name",
			unique: true);
	}
}
