using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Notification.Infrastructure.Migrations;

public partial class NotificationIdToNullable : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_notification_history_notification_notification_id",
			table: "notification_history");

		migrationBuilder.DropForeignKey(
			name: "fk_notification_push_history_notification_notification_id",
			table: "notification_push_history");

		migrationBuilder.AlterColumn<Guid>(
			name: "notification_id",
			table: "notification_push_history",
			type: "uuid",
			nullable: true,
			oldClrType: typeof(Guid),
			oldType: "uuid");

		migrationBuilder.AlterColumn<Guid>(
			name: "notification_id",
			table: "notification_history",
			type: "uuid",
			nullable: true,
			oldClrType: typeof(Guid),
			oldType: "uuid");

		migrationBuilder.AddForeignKey(
			name: "fk_notification_history_notification_notification_id",
			table: "notification_history",
			column: "notification_id",
			principalTable: "notification",
			principalColumn: "id");

		migrationBuilder.AddForeignKey(
			name: "fk_notification_push_history_notification_notification_id",
			table: "notification_push_history",
			column: "notification_id",
			principalTable: "notification",
			principalColumn: "id");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_notification_history_notification_notification_id",
			table: "notification_history");

		migrationBuilder.DropForeignKey(
			name: "fk_notification_push_history_notification_notification_id",
			table: "notification_push_history");

		migrationBuilder.AlterColumn<Guid>(
			name: "notification_id",
			table: "notification_push_history",
			type: "uuid",
			nullable: false,
			defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
			oldClrType: typeof(Guid),
			oldType: "uuid",
			oldNullable: true);

		migrationBuilder.AlterColumn<Guid>(
			name: "notification_id",
			table: "notification_history",
			type: "uuid",
			nullable: false,
			defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
			oldClrType: typeof(Guid),
			oldType: "uuid",
			oldNullable: true);

		migrationBuilder.AddForeignKey(
			name: "fk_notification_history_notification_notification_id",
			table: "notification_history",
			column: "notification_id",
			principalTable: "notification",
			principalColumn: "id",
			onDelete: ReferentialAction.Cascade);

		migrationBuilder.AddForeignKey(
			name: "fk_notification_push_history_notification_notification_id",
			table: "notification_push_history",
			column: "notification_id",
			principalTable: "notification",
			principalColumn: "id",
			onDelete: ReferentialAction.Cascade);
	}
}
