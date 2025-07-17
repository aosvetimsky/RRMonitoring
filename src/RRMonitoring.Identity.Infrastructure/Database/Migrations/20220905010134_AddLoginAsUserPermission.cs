using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddLoginAsUserPermission : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql(@"
                DO $$

                DECLARE
                    permissionGroupId int;

                BEGIN
                    SELECT id INTO permissionGroupId FROM permission_group WHERE name = 'Администрирование';

                    INSERT INTO permission(id, name, display_name, group_id)
                    VALUES(
                            uuid_generate_v4(),
                            'login_as_user',
                            'Логин за другого пользователя',
                            permissionGroupId);
                END $$
            ");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{

	}
}
