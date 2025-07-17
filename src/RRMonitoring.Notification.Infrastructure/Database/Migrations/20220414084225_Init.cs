using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RRMonitoring.Notification.Infrastructure.Migrations;

public partial class Init : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "channel",
			columns: table => new
			{
				id = table.Column<byte>(type: "smallint", nullable: false),
				name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_channel", x => x.id);
			});

		migrationBuilder.CreateTable(
			name: "notification_group",
			columns: table => new
			{
				id = table.Column<int>(type: "integer", nullable: false)
					.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
				name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_notification_group", x => x.id);
			});

		migrationBuilder.CreateTable(
			name: "status",
			columns: table => new
			{
				id = table.Column<byte>(type: "smallint", nullable: false),
				name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_status", x => x.id);
			});

		migrationBuilder.CreateTable(
			name: "notification_type",
			columns: table => new
			{
				id = table.Column<int>(type: "integer", nullable: false)
					.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
				name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
				group_id = table.Column<int>(type: "integer", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_notification_type", x => x.id);
				table.ForeignKey(
					name: "fk_notification_type_notification_group_group_id",
					column: x => x.group_id,
					principalTable: "notification_group",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateTable(
			name: "notification",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				identifier = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
				type_id = table.Column<int>(type: "integer", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_notification", x => x.id);
				table.ForeignKey(
					name: "fk_notification_notification_type_type_id",
					column: x => x.type_id,
					principalTable: "notification_type",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateTable(
			name: "notification_history",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				recipient_id = table.Column<Guid>(type: "uuid", nullable: false),
				recipient_address = table.Column<string>(type: "text", nullable: false),
				status_id = table.Column<byte>(type: "smallint", nullable: false),
				notification_body = table.Column<string>(type: "text", nullable: true),
				channel_id = table.Column<byte>(type: "smallint", nullable: false),
				notification_id = table.Column<Guid>(type: "uuid", nullable: false),
				created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				created_by = table.Column<Guid>(type: "uuid", nullable: false),
				updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
				updated_by = table.Column<Guid>(type: "uuid", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_notification_history", x => x.id);
				table.ForeignKey(
					name: "fk_notification_history_channel_channel_id",
					column: x => x.channel_id,
					principalTable: "channel",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "fk_notification_history_notification_notification_id",
					column: x => x.notification_id,
					principalTable: "notification",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "fk_notification_history_status_status_id",
					column: x => x.status_id,
					principalTable: "status",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateTable(
			name: "recipient_setting",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				recipient_id = table.Column<Guid>(type: "uuid", nullable: false),
				channel_id = table.Column<byte>(type: "smallint", nullable: false),
				notification_id = table.Column<Guid>(type: "uuid", nullable: false),
				is_enabled = table.Column<bool>(type: "boolean", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_recipient_setting", x => x.id);
				table.ForeignKey(
					name: "fk_recipient_setting_channel_channel_id",
					column: x => x.channel_id,
					principalTable: "channel",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "fk_recipient_setting_notification_notification_id",
					column: x => x.notification_id,
					principalTable: "notification",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateTable(
			name: "template",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				channel_id = table.Column<byte>(type: "smallint", nullable: false),
				notification_id = table.Column<Guid>(type: "uuid", nullable: false),
				data = table.Column<string>(type: "text", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_template", x => x.id);
				table.ForeignKey(
					name: "fk_template_channel_channel_id",
					column: x => x.channel_id,
					principalTable: "channel",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "fk_template_notification_notification_id",
					column: x => x.notification_id,
					principalTable: "notification",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "ix_notification_type_id",
			table: "notification",
			column: "type_id");

		migrationBuilder.CreateIndex(
			name: "ix_notification_history_channel_id",
			table: "notification_history",
			column: "channel_id");

		migrationBuilder.CreateIndex(
			name: "ix_notification_history_notification_id",
			table: "notification_history",
			column: "notification_id");

		migrationBuilder.CreateIndex(
			name: "ix_notification_history_status_id",
			table: "notification_history",
			column: "status_id");

		migrationBuilder.CreateIndex(
			name: "ix_notification_type_group_id",
			table: "notification_type",
			column: "group_id");

		migrationBuilder.CreateIndex(
			name: "ix_recipient_setting_channel_id",
			table: "recipient_setting",
			column: "channel_id");

		migrationBuilder.CreateIndex(
			name: "ix_recipient_setting_notification_id",
			table: "recipient_setting",
			column: "notification_id");

		migrationBuilder.CreateIndex(
			name: "ix_template_channel_id",
			table: "template",
			column: "channel_id");

		migrationBuilder.CreateIndex(
			name: "ix_template_notification_id",
			table: "template",
			column: "notification_id");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "notification_history");

		migrationBuilder.DropTable(
			name: "recipient_setting");

		migrationBuilder.DropTable(
			name: "template");

		migrationBuilder.DropTable(
			name: "status");

		migrationBuilder.DropTable(
			name: "channel");

		migrationBuilder.DropTable(
			name: "notification");

		migrationBuilder.DropTable(
			name: "notification_type");

		migrationBuilder.DropTable(
			name: "notification_group");
	}
}
