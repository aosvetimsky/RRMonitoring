using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddTenantCode : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<string>(
			name: "code",
			table: "tenant",
			type: "character varying(50)",
			maxLength: 50,
			nullable: true);

		migrationBuilder.CreateIndex(
			name: "ix_tenant_code",
			table: "tenant",
			column: "code",
			unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "ix_tenant_code",
			table: "tenant");

		migrationBuilder.DropColumn(
			name: "code",
			table: "tenant");
	}
}
