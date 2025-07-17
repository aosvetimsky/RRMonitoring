using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddIsAuthenticatorEnabledFlag : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<bool>(
			name: "is_authenticator_enabled",
			table: "user",
			type: "boolean",
			nullable: false,
			defaultValue: false);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "is_authenticator_enabled",
			table: "user");
	}
}
