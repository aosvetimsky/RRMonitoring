using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddCreatedAndUpdatedDateToUser : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<DateTime>(
			name: "created_date",
			table: "user",
			type: "timestamp with time zone",
			nullable: true);

		migrationBuilder.Sql(@"
                UPDATE public.user
                SET created_date = timezone('utc', now());"
		);

		migrationBuilder.AlterColumn<DateTime>(
			name: "created_date",
			table: "user",
			nullable: false,
			oldNullable: true);

		migrationBuilder.AddColumn<DateTime>(
			name: "updated_date",
			table: "user",
			type: "timestamp with time zone",
			nullable: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "created_date",
			table: "user");

		migrationBuilder.DropColumn(
			name: "updated_date",
			table: "user");
	}
}
