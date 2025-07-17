using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AdditionalUserProperties : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<bool>(
			name: "is_admin",
			table: "user",
			type: "boolean",
			nullable: false,
			defaultValue: false);

		migrationBuilder.AddColumn<bool>(
			name: "is_blocked",
			table: "user",
			type: "boolean",
			nullable: false,
			defaultValue: false);

		migrationBuilder.AddColumn<string>(
			name: "middle_name",
			table: "user",
			type: "character varying(50)",
			maxLength: 50,
			nullable: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "is_admin",
			table: "user");

		migrationBuilder.DropColumn(
			name: "is_blocked",
			table: "user");

		migrationBuilder.DropColumn(
			name: "middle_name",
			table: "user");
	}
}
