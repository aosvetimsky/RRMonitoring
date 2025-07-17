using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RRMonitoring.Notification.Infrastructure.Migrations;

public partial class RemoveNotificationType : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_notification_notification_type_type_id",
			table: "notification");

		migrationBuilder.DropIndex(
			name: "ix_notification_type_id",
			table: "notification"
		);

		migrationBuilder.AddColumn<string>(
			name: "description",
			table: "notification",
			type: "character varying(250)",
			maxLength: 250,
			nullable: true);

		migrationBuilder.AddColumn<int>(
			name: "group_id",
			table: "notification",
			type: "integer",
			nullable: true);

		migrationBuilder.Sql(@"
                UPDATE notification
                SET description = notification_type.name,
                    group_id = notification_type.group_id
                FROM notification_type
                WHERE notification_type.id = notification.type_id;
            ");

		migrationBuilder.AlterColumn<int>(
			name: "group_id",
			table: "notification",
			nullable: false);

		migrationBuilder.DropTable(
			name: "notification_type");

		migrationBuilder.DropColumn(
			 name: "type_id",
			 table: "notification");

		migrationBuilder.AddForeignKey(
			name: "fk_notification_notification_group_group_id",
			table: "notification",
			column: "group_id",
			principalTable: "notification_group",
			principalColumn: "id",
			onDelete: ReferentialAction.Restrict);

		migrationBuilder.CreateIndex(
			name: "ix_notification_group_id",
			table: "notification",
			column: "group_id");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_notification_notification_group_group_id",
			table: "notification");

		migrationBuilder.DropIndex(
			name: "ix_notification_group_id",
			table: "notification"
		);

		migrationBuilder.AddColumn<int>(
			name: "type_id",
			table: "notification",
			type: "integer",
			nullable: true);

		migrationBuilder.CreateTable(
			name: "notification_type",
			columns: table => new
			{
				id = table.Column<int>(type: "integer", nullable: false)
					.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
				group_id = table.Column<int>(type: "integer", nullable: false),
				name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
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

		migrationBuilder.Sql(@"
                INSERT INTO notification_type (name, group_id)
                SELECT description, group_id FROM notification;
            ");

		migrationBuilder.Sql(@"
                UPDATE notification
                SET type_id = notification_type.id
                FROM notification_type
                WHERE notification_type.name = notification.description and notification_type.group_id = notification.group_id;
            ");

		migrationBuilder.AlterColumn<int>(
			name: "type_id",
			table: "notification",
			nullable: false);

		migrationBuilder.DropColumn(
			name: "description",
			table: "notification");

		migrationBuilder.DropColumn(
			name: "group_id",
			table: "notification");

		migrationBuilder.CreateIndex(
			name: "ix_notification_type_group_id",
			table: "notification_type",
			column: "group_id");

		migrationBuilder.AddForeignKey(
			name: "fk_notification_notification_type_type_id",
			table: "notification",
			column: "type_id",
			principalTable: "notification_type",
			principalColumn: "id",
			onDelete: ReferentialAction.Cascade);

		migrationBuilder.CreateIndex(
			name: "ix_notification_type_id",
			table: "notification",
			column: "type_id");
	}
}
