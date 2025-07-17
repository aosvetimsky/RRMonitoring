using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddUserBlockPermissionToScope : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql(@"
                DO $$

                DECLARE
                    scopeId int;
                    permissionId uuid;
                BEGIN
                    SELECT id INTO scopeId FROM api_scope WHERE name = 'identity_api';
                    SELECT id INTO permissionId FROM permission WHERE name = 'user_block';

                    INSERT INTO scope_permission (scope_id, permission_id)
                    VALUES (scopeId, permissionId);
                END $$;
            ");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{

	}
}
