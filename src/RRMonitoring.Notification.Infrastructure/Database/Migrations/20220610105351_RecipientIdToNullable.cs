﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Notification.Infrastructure.Migrations;

public partial class RecipientIdToNullable : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<Guid>(
			name: "recipient_id",
			table: "notification_history",
			type: "uuid",
			nullable: true,
			oldClrType: typeof(Guid),
			oldType: "uuid");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<Guid>(
			name: "recipient_id",
			table: "notification_history",
			type: "uuid",
			nullable: false,
			defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
			oldClrType: typeof(Guid),
			oldType: "uuid",
			oldNullable: true);
	}
}
