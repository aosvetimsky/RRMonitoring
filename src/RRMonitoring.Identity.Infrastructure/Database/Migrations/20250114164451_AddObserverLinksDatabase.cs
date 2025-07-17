using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddObserverLinksDatabase : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "api_key_type",
			columns: table => new
			{
				id = table.Column<byte>(type: "smallint", nullable: false),
				name = table.Column<string>(type: "text", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_api_key_type", x => x.id);
			});

		migrationBuilder.CreateTable(
			name: "external_permission",
			columns: table => new
			{
				id = table.Column<byte>(type: "smallint", nullable: false),
				code = table.Column<string>(type: "text", nullable: true),
				name = table.Column<string>(type: "text", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_external_permission", x => x.id);
			});

		migrationBuilder.CreateTable(
			name: "api_key",
			columns: table => new
			{
				key = table.Column<string>(type: "text", nullable: false),
				account_id = table.Column<Guid>(type: "uuid", nullable: true),
				description = table.Column<string>(type: "text", nullable: true),
				ip_whitelist = table.Column<List<string>>(type: "text[]", nullable: true),
				type_id = table.Column<byte>(type: "smallint", nullable: false),
				created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
				created_by = table.Column<Guid>(type: "uuid", nullable: false),
				updated_by = table.Column<Guid>(type: "uuid", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_api_key", x => x.key);
				table.ForeignKey(
					name: "fk_api_key_account_account_id",
					column: x => x.account_id,
					principalTable: "account",
					principalColumn: "id",
					onDelete: ReferentialAction.Restrict);
				table.ForeignKey(
					name: "fk_api_key_api_key_type_type_id",
					column: x => x.type_id,
					principalTable: "api_key_type",
					principalColumn: "id",
					onDelete: ReferentialAction.Restrict);
			});

		migrationBuilder.CreateTable(
			name: "api_key_permission",
			columns: table => new
			{
				key = table.Column<string>(type: "text", nullable: false),
				external_permission_id = table.Column<byte>(type: "smallint", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_api_key_permission", x => new { x.key, x.external_permission_id });
				table.ForeignKey(
					name: "fk_api_key_permission_api_key_api_key_key",
					column: x => x.key,
					principalTable: "api_key",
					principalColumn: "key",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "fk_api_key_permission_external_permission_external_permission_",
					column: x => x.external_permission_id,
					principalTable: "external_permission",
					principalColumn: "id",
					onDelete: ReferentialAction.Restrict);
			});

		migrationBuilder.InsertData(
			table: "api_key_type",
			columns: new[] { "id", "name" },
			values: new object[,]
			{
				{ (byte)1, "OpenApi" },
				{ (byte)2, "Observer" }
			});

		migrationBuilder.InsertData(
			table: "external_permission",
			columns: new[] { "id", "code", "name" },
			values: new object[,]
			{
				{ (byte)1, "mining", "Майнинг" },
				{ (byte)2, "assets", "Активы" },
				{ (byte)3, "profit_history", "История прибыли" }
			});

		migrationBuilder.CreateIndex(
			name: "ix_api_key_account_id",
			table: "api_key",
			column: "account_id");

		migrationBuilder.CreateIndex(
			name: "ix_api_key_type_id",
			table: "api_key",
			column: "type_id");

		migrationBuilder.CreateIndex(
			name: "ix_api_key_permission_external_permission_id",
			table: "api_key_permission",
			column: "external_permission_id");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "api_key_permission");

		migrationBuilder.DropTable(
			name: "api_key");

		migrationBuilder.DropTable(
			name: "external_permission");

		migrationBuilder.DropTable(
			name: "api_key_type");
	}
}
