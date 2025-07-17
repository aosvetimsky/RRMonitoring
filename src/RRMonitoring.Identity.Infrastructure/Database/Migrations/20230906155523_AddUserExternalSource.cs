using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddUserExternalSource : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<byte>(
			name: "external_source_id",
			table: "user",
			type: "smallint",
			nullable: true);

		migrationBuilder.CreateTable(
			name: "external_source",
			columns: table => new
			{
				id = table.Column<byte>(type: "smallint", nullable: false),
				code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
				name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_external_source", x => x.id);
			});

		migrationBuilder.InsertData(
			table: "external_source",
			columns: new[] { "id", "code", "name" },
			values: new object[] { (byte)1, "azure_ad", "Azure AD" });

		migrationBuilder.CreateIndex(
			name: "ix_user_external_source_id",
			table: "user",
			column: "external_source_id");

		migrationBuilder.CreateIndex(
			name: "ix_external_source_code",
			table: "external_source",
			column: "code",
			unique: true);

		migrationBuilder.AddForeignKey(
			name: "fk_user_external_source_external_source_id",
			table: "user",
			column: "external_source_id",
			principalTable: "external_source",
			principalColumn: "id",
			onDelete: ReferentialAction.Restrict);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_user_external_source_external_source_id",
			table: "user");

		migrationBuilder.DropTable(
			name: "external_source");

		migrationBuilder.DropIndex(
			name: "ix_user_external_source_id",
			table: "user");

		migrationBuilder.DropColumn(
			name: "external_source_id",
			table: "user");
	}
}
