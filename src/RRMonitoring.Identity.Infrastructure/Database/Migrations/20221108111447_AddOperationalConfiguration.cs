using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddOperationalConfiguration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "device_flow_code",
			columns: table => new
			{
				user_code = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
				device_code = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
				subject_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
				session_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
				client_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
				description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
				creation_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_device_flow_code", x => x.user_code);
			});

		migrationBuilder.CreateTable(
			name: "persisted_grant",
			columns: table => new
			{
				key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
				type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
				subject_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
				session_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
				client_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
				description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
				creation_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
				consumed_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
				data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_persisted_grant", x => x.key);
			});

		migrationBuilder.CreateIndex(
			name: "ix_device_flow_code_device_code",
			table: "device_flow_code",
			column: "device_code",
			unique: true);

		migrationBuilder.CreateIndex(
			name: "ix_device_flow_code_expiration",
			table: "device_flow_code",
			column: "expiration");

		migrationBuilder.CreateIndex(
			name: "ix_persisted_grant_expiration",
			table: "persisted_grant",
			column: "expiration");

		migrationBuilder.CreateIndex(
			name: "ix_persisted_grant_subject_id_client_id_type",
			table: "persisted_grant",
			columns: new[] { "subject_id", "client_id", "type" });

		migrationBuilder.CreateIndex(
			name: "ix_persisted_grant_subject_id_session_id_type",
			table: "persisted_grant",
			columns: new[] { "subject_id", "session_id", "type" });
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "device_flow_code");

		migrationBuilder.DropTable(
			name: "persisted_grant");
	}
}
