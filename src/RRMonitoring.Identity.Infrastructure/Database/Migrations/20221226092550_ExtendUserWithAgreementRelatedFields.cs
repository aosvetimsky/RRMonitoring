using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class ExtendUserWithAgreementRelatedFields : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<DateTime>(
			name: "agreement_accepted_date",
			table: "user",
			type: "timestamp with time zone",
			nullable: true);

		migrationBuilder.AddColumn<bool>(
			name: "is_agreement_acceptance_required",
			table: "user",
			type: "boolean",
			nullable: false,
			defaultValue: false);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "agreement_accepted_date",
			table: "user");

		migrationBuilder.DropColumn(
			name: "is_agreement_acceptance_required",
			table: "user");
	}
}
