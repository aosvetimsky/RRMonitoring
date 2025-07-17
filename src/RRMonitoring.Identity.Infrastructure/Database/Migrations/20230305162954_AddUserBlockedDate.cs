using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddUserBlockedDate : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<DateTime>(
			name: "blocked_date",
			table: "user",
			type: "timestamp with time zone",
			nullable: true);

		migrationBuilder.Sql(@"
                DO $$

                DECLARE
                    groupId int;

                BEGIN
                    SELECT id INTO groupId FROM permission_group WHERE name = 'Администрирование';

                    INSERT INTO permission(id, name, display_name, group_id)
                    VALUES(
                            uuid_generate_v4(),
                            'user_block',
                            'Блокировка пользователя',
                            groupId);
                END $$;
            ");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "blocked_date",
			table: "user");
	}
}
