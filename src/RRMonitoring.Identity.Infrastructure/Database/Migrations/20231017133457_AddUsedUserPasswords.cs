using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddUsedUserPasswords : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "used_user_password",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				user_id = table.Column<Guid>(type: "uuid", nullable: false),
				password_hash = table.Column<string>(type: "text", nullable: true),
				created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_used_user_password", x => x.id);
				table.ForeignKey(
					name: "fk_used_user_password_users_user_id",
					column: x => x.user_id,
					principalTable: "user",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "ix_used_user_password_user_id",
			table: "used_user_password",
			column: "user_id");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "used_user_password");
	}
}
