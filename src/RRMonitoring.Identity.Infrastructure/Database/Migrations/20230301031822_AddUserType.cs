using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddUserType : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<byte>(
			name: "type_id",
			table: "user",
			type: "smallint",
			nullable: true);

		migrationBuilder.CreateTable(
			name: "user_type",
			columns: table => new
			{
				id = table.Column<byte>(type: "smallint", nullable: false),
				name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
				code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_user_type", x => x.id);
			});

		migrationBuilder.CreateIndex(
			name: "ix_user_type_id",
			table: "user",
			column: "type_id");

		migrationBuilder.CreateIndex(
			name: "ix_user_type_code",
			table: "user_type",
			column: "code",
			unique: true);

		migrationBuilder.AddForeignKey(
			name: "fk_user_user_type_type_id",
			table: "user",
			column: "type_id",
			principalTable: "user_type",
			principalColumn: "id",
			onDelete: ReferentialAction.Restrict);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_user_user_type_type_id",
			table: "user");

		migrationBuilder.DropTable(
			name: "user_type");

		migrationBuilder.DropIndex(
			name: "ix_user_type_id",
			table: "user");

		migrationBuilder.DropColumn(
			name: "type_id",
			table: "user");
	}
}
