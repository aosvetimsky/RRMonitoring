using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddSigningKeys : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "signing_key",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				creation_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
				value = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_signing_key", x => x.id);
			});
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "signing_key");
	}
}
