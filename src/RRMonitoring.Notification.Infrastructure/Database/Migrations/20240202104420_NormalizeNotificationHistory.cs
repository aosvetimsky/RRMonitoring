using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Notification.Infrastructure.Migrations;

/// <inheritdoc />
public partial class NormalizeNotificationHistory : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "notification_message",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				recipient_id = table.Column<string>(type: "text", nullable: true),
				recipient_address = table.Column<string>(type: "text", nullable: false),
				external_message_id = table.Column<string>(type: "text", nullable: true),
				notification_body = table.Column<string>(type: "text", nullable: true),
				channel_id = table.Column<byte>(type: "smallint", nullable: false),
				notification_id = table.Column<Guid>(type: "uuid", nullable: true),
				push_is_read = table.Column<bool>(type: "boolean", nullable: true),
				created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				created_by = table.Column<Guid>(type: "uuid", nullable: false),
				updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
				updated_by = table.Column<Guid>(type: "uuid", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_notification_message", x => x.id);
				table.ForeignKey(
					name: "fk_notification_message_channel_channel_id",
					column: x => x.channel_id,
					principalTable: "channel",
					principalColumn: "id",
					onDelete: ReferentialAction.Restrict);
				table.ForeignKey(
					name: "fk_notification_message_notification_notification_id",
					column: x => x.notification_id,
					principalTable: "notification",
					principalColumn: "id",
					onDelete: ReferentialAction.Restrict);
			});

		migrationBuilder.CreateTable(
			name: "notification_message_history",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				notification_message_id = table.Column<Guid>(type: "uuid", nullable: false),
				status_id = table.Column<byte>(type: "smallint", nullable: false),
				error_text = table.Column<string>(type: "text", nullable: true),
				external_system_status = table.Column<string>(type: "text", nullable: true),
				created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
				created_by = table.Column<Guid>(type: "uuid", nullable: false),
				updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
				updated_by = table.Column<Guid>(type: "uuid", nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_notification_message_history", x => x.id);
				table.ForeignKey(
					name: "fk_notification_message_history_notification_message_notificat",
					column: x => x.notification_message_id,
					principalTable: "notification_message",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "fk_notification_message_history_status_status_id",
					column: x => x.status_id,
					principalTable: "status",
					principalColumn: "id",
					onDelete: ReferentialAction.Restrict);
			});

		migrationBuilder.CreateIndex(
			name: "ix_notification_message_channel_id",
			table: "notification_message",
			column: "channel_id");

		migrationBuilder.CreateIndex(
			name: "ix_notification_message_external_message_id",
			table: "notification_message",
			column: "external_message_id");

		migrationBuilder.CreateIndex(
			name: "ix_notification_message_notification_id",
			table: "notification_message",
			column: "notification_id");

		migrationBuilder.CreateIndex(
			name: "ix_notification_message_history_notification_message_id",
			table: "notification_message_history",
			column: "notification_message_id");

		migrationBuilder.CreateIndex(
			name: "ix_notification_message_history_status_id",
			table: "notification_message_history",
			column: "status_id");

		migrationBuilder.Sql(
			"""
                INSERT INTO notification_message (
                    id,
                    recipient_id, recipient_address, channel_id, notification_id, external_message_id, notification_body,
                    push_is_read,
                    created_date,
                    created_by
                )
                SELECT
                    gen_random_uuid(),
                    nh.recipient_id, nh.recipient_address, nh.channel_id, nh.notification_id, nh.external_message_id, nh.notification_body,
                    (
                        SELECT
                            MAX(nph.is_read::int)::bool
                        FROM
                            notification_push_history nph
                        WHERE
                            (nph.notification_id, nph.recipient_id, nph.external_message_id, nph.notification_body) IS NOT DISTINCT FROM
                            (nh.notification_id, nh.recipient_id, nh.external_message_id, nh.notification_body)
                    ) AS push_is_read,
                    MIN(nh.created_date),
                    MAX(nh.created_by::text)::uuid
                FROM
                    notification_history nh
                GROUP BY
                    nh.recipient_id, nh.recipient_address, nh.channel_id, nh.notification_id, nh.external_message_id, nh.notification_body
                """);

		migrationBuilder.Sql(
			"""
                INSERT INTO notification_message_history (
                    id,
                    notification_message_id,
                    status_id,
                    error_text,
                    external_system_status,
                    created_date,
                    created_by
                )
                SELECT
                    nh.id,
                    nm.id,
                    nh.status_id,
                    nh.error_text,
                    nh.external_system_status,
                    nh.created_date,
                    nh.created_by
                FROM
                    notification_history nh
                    JOIN notification_message nm ON
                        ARRAY[nm.recipient_id, nm.recipient_address, nm.channel_id::text, nm.notification_id::text, nm.external_message_id, nm.notification_body] =
                        ARRAY[nh.recipient_id, nh.recipient_address, nh.channel_id::text, nh.notification_id::text, nh.external_message_id, nh.notification_body]
                """);

		migrationBuilder.DropTable(
			name: "notification_history");

		migrationBuilder.DropTable(
			name: "notification_push_history");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		throw new NotSupportedException();
	}
}
