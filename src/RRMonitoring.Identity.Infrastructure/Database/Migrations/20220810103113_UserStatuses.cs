using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class UserStatuses : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<byte>(
			name: "status_id",
			table: "user",
			type: "smallint",
			nullable: false,
			defaultValue: (byte)2);

		migrationBuilder.CreateTable(
			name: "user_status",
			columns: table => new
			{
				id = table.Column<byte>(type: "smallint", nullable: false),
				name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_user_status", x => x.id);
			});

		migrationBuilder.InsertData(
			table: "user_status",
			columns: new[] { "id", "name" },
			values: new object[,]
			{
				{ (byte)1, "Запрос на подключение" },
				{ (byte)2, "Активный" },
				{ (byte)3, "Заблокированный" }
			});

		migrationBuilder.CreateIndex(
			name: "ix_user_status_id",
			table: "user",
			column: "status_id");

		migrationBuilder.AddForeignKey(
			name: "fk_user_user_status_status_id",
			table: "user",
			column: "status_id",
			principalTable: "user_status",
			principalColumn: "id",
			onDelete: ReferentialAction.Restrict);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_user_user_status_status_id",
			table: "user");

		migrationBuilder.DropTable(
			name: "user_status");

		migrationBuilder.DropIndex(
			name: "ix_user_status_id",
			table: "user");

		migrationBuilder.DropColumn(
			name: "status_id",
			table: "user");
	}
}
