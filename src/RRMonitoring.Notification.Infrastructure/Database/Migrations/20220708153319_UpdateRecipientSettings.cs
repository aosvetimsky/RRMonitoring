using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Notification.Infrastructure.Migrations;

public partial class UpdateRecipientSettings : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_recipient_setting_notification_notification_id",
			table: "recipient_setting");

		migrationBuilder.DropPrimaryKey(
			name: "pk_recipient_setting",
			table: "recipient_setting");

		migrationBuilder.DropIndex(
			name: "ix_recipient_setting_notification_id",
			table: "recipient_setting");

		migrationBuilder.DropColumn(
			name: "id",
			table: "recipient_setting");

		migrationBuilder.DropColumn(
			name: "notification_id",
			table: "recipient_setting");

		migrationBuilder.AlterColumn<string>(
			name: "recipient_id",
			table: "recipient_setting",
			type: "text",
			nullable: false,
			oldClrType: typeof(Guid),
			oldType: "uuid");

		migrationBuilder.AddColumn<string>(
			name: "notification_identifier",
			table: "recipient_setting",
			type: "character varying(250)",
			nullable: false,
			defaultValue: "");

		migrationBuilder.AlterColumn<string>(
			name: "recipient_id",
			table: "push_registered_device",
			type: "text",
			nullable: false,
			oldClrType: typeof(Guid),
			oldType: "uuid");

		migrationBuilder.AlterColumn<string>(
			name: "recipient_id",
			table: "notification_history",
			type: "text",
			nullable: true,
			oldClrType: typeof(Guid),
			oldType: "uuid",
			oldNullable: true);

		migrationBuilder.AddPrimaryKey(
			name: "pk_recipient_setting",
			table: "recipient_setting",
			columns: new[] { "recipient_id", "channel_id", "notification_identifier" });

		migrationBuilder.AddUniqueConstraint(
			name: "ak_notification_identifier",
			table: "notification",
			column: "identifier");

		migrationBuilder.CreateIndex(
			name: "ix_recipient_setting_notification_identifier",
			table: "recipient_setting",
			column: "notification_identifier");

		migrationBuilder.AddForeignKey(
			name: "fk_recipient_setting_notification_notification_id",
			table: "recipient_setting",
			column: "notification_identifier",
			principalTable: "notification",
			principalColumn: "identifier",
			onDelete: ReferentialAction.Cascade);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_recipient_setting_notification_notification_id",
			table: "recipient_setting");

		migrationBuilder.DropPrimaryKey(
			name: "pk_recipient_setting",
			table: "recipient_setting");

		migrationBuilder.DropIndex(
			name: "ix_recipient_setting_notification_identifier",
			table: "recipient_setting");

		migrationBuilder.DropUniqueConstraint(
			name: "ak_notification_identifier",
			table: "notification");

		migrationBuilder.DropColumn(
			name: "notification_identifier",
			table: "recipient_setting");

		migrationBuilder.Sql("ALTER TABLE recipient_setting ALTER COLUMN recipient_id SET DATA TYPE UUID USING (uuid_generate_v4());");

		migrationBuilder.Sql("ALTER TABLE recipient_setting ADD COLUMN id uuid DEFAULT uuid_generate_v1();");

		migrationBuilder.AddColumn<Guid>(
			name: "notification_id",
			table: "recipient_setting",
			type: "uuid",
			nullable: false,
			defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

		migrationBuilder.Sql("ALTER TABLE push_registered_device ALTER COLUMN recipient_id SET DATA TYPE UUID USING (uuid_generate_v4());");

		migrationBuilder.Sql("ALTER TABLE notification_history ALTER COLUMN recipient_id SET DATA TYPE UUID USING (uuid_generate_v4());");

		migrationBuilder.AddPrimaryKey(
			name: "pk_recipient_setting",
			table: "recipient_setting",
			column: "id");

		migrationBuilder.CreateIndex(
			name: "ix_recipient_setting_notification_id",
			table: "recipient_setting",
			column: "notification_id");

		migrationBuilder.AddForeignKey(
			name: "fk_recipient_setting_notification_notification_id",
			table: "recipient_setting",
			column: "notification_id",
			principalTable: "notification",
			principalColumn: "id",
			onDelete: ReferentialAction.Cascade);
	}
}
