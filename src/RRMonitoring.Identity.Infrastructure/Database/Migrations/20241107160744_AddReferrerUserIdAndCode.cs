using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddReferrerUserIdAndCode : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<string>(
			name: "referral_code",
			table: "user",
			type: "text",
			nullable: false,
			defaultValue: "");

		migrationBuilder.Sql(
			@"
	            WITH numbered_users AS (
				    SELECT id, ROW_NUMBER() OVER (ORDER BY created_date) AS row_num
				    FROM ""user""
				)
				UPDATE ""user""
				SET referral_code = numbered_users.row_num::text
				FROM numbered_users
				WHERE ""user"".id = numbered_users.id;
	            ");

		migrationBuilder.AddColumn<Guid>(
			name: "referrer_user_id",
			table: "user",
			type: "uuid",
			nullable: true);

		migrationBuilder.CreateIndex(
			name: "ix_user_referral_code",
			table: "user",
			column: "referral_code",
			unique: true);

		migrationBuilder.CreateIndex(
			name: "ix_user_referrer_user_id",
			table: "user",
			column: "referrer_user_id");

		migrationBuilder.AddForeignKey(
			name: "fk_user_user_referrer_user_id",
			table: "user",
			column: "referrer_user_id",
			principalTable: "user",
			principalColumn: "id");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_user_user_referrer_user_id",
			table: "user");

		migrationBuilder.DropIndex(
			name: "ix_user_referral_code",
			table: "user");

		migrationBuilder.DropIndex(
			name: "ix_user_referrer_user_id",
			table: "user");

		migrationBuilder.DropColumn(
			name: "referral_code",
			table: "user");

		migrationBuilder.DropColumn(
			name: "referrer_user_id",
			table: "user");
	}
}
