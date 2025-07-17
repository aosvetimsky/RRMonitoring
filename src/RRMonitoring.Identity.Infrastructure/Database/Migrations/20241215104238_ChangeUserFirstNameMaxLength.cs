﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class ChangeUserFirstNameMaxLength : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<string>(
			name: "first_name",
			table: "user",
			type: "character varying(30)",
			maxLength: 30,
			nullable: false,
			oldClrType: typeof(string),
			oldType: "character varying(20)",
			oldMaxLength: 20);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<string>(
			name: "first_name",
			table: "user",
			type: "character varying(20)",
			maxLength: 20,
			nullable: false,
			oldClrType: typeof(string),
			oldType: "character varying(30)",
			oldMaxLength: 30);
	}
}
