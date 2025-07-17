using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddTenantTables : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "RoleNameIndex",
			table: "role");

		migrationBuilder.AddColumn<string>(
			name: "code",
			table: "role",
			type: "character varying(50)",
			maxLength: 50,
			nullable: true);

		migrationBuilder.AddColumn<bool>(
			name: "is_system",
			table: "role",
			type: "boolean",
			nullable: false,
			defaultValue: false);

		migrationBuilder.AddColumn<Guid>(
			name: "tenant_id",
			table: "role",
			type: "uuid",
			nullable: true);

		migrationBuilder.CreateTable(
			name: "tenant",
			columns: table => new
			{
				id = table.Column<Guid>(type: "uuid", nullable: false),
				name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_tenant", x => x.id);
			});

		migrationBuilder.CreateTable(
			name: "tenant_user",
			columns: table => new
			{
				tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
				user_id = table.Column<Guid>(type: "uuid", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("pk_tenant_user", x => new { x.tenant_id, x.user_id });
				table.ForeignKey(
					name: "fk_tenant_user_tenant_tenant_id",
					column: x => x.tenant_id,
					principalTable: "tenant",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
				table.ForeignKey(
					name: "fk_tenant_user_users_user_id",
					column: x => x.user_id,
					principalTable: "user",
					principalColumn: "id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "ix_role_code",
			table: "role",
			column: "code",
			unique: true,
			filter: "tenant_id IS NULL");

		migrationBuilder.CreateIndex(
			name: "ix_role_code_tenant_id",
			table: "role",
			columns: new[] { "code", "tenant_id" },
			unique: true);

		migrationBuilder.CreateIndex(
			name: "ix_role_normalized_name_tenant_id",
			table: "role",
			columns: new[] { "normalized_name", "tenant_id" },
			unique: true);

		migrationBuilder.CreateIndex(
			name: "ix_role_tenant_id",
			table: "role",
			column: "tenant_id");

		migrationBuilder.CreateIndex(
			name: "RoleNameIndex",
			table: "role",
			column: "normalized_name",
			unique: true,
			filter: "tenant_id IS NULL");

		migrationBuilder.CreateIndex(
			name: "ix_tenant_name",
			table: "tenant",
			column: "name");

		migrationBuilder.CreateIndex(
			name: "ix_tenant_user_user_id",
			table: "tenant_user",
			column: "user_id");

		migrationBuilder.AddForeignKey(
			name: "fk_role_tenant_tenant_id",
			table: "role",
			column: "tenant_id",
			principalTable: "tenant",
			principalColumn: "id");

		migrationBuilder.Sql(@"
                CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";

                DO $$

                DECLARE
                    scopeId int;
                    permissionGroupId int;
                    permissionId uuid;

                BEGIN
                    -- Группа администрирования
                    SELECT id INTO permissionGroupId
                    FROM permission_group
                    WHERE name = 'Администрирование'
                    LIMIT 1;

                    SELECT id INTO scopeId
                    FROM api_scope
                    WHERE name = 'identity_api'
                    ORDER BY name
                    LIMIT 1;

                    INSERT INTO permission(id, name, display_name, group_id)
                    VALUES(
                            uuid_generate_v4(),
                            'tenant_read',
                            'Просмотр справочника клиентов',
                            permissionGroupId
                        )
                    RETURNING id INTO permissionId;

                    INSERT INTO scope_permission(scope_id, permission_id)
                    VALUES(scopeId, permissionId);

                    INSERT INTO permission(id, name, display_name, group_id)
                    VALUES(
                            uuid_generate_v4(),
                            'tenant_manage',
                            'Редактирование, создание, удаление клиентов',
                            permissionGroupId
                        )
                    RETURNING id INTO permissionId;

                    INSERT INTO scope_permission(scope_id, permission_id)
                    VALUES(scopeId, permissionId);

                END $$
            ");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "fk_role_tenant_tenant_id",
			table: "role");

		migrationBuilder.DropTable(
			name: "tenant_user");

		migrationBuilder.DropTable(
			name: "tenant");

		migrationBuilder.DropIndex(
			name: "ix_role_code",
			table: "role");

		migrationBuilder.DropIndex(
			name: "ix_role_code_tenant_id",
			table: "role");

		migrationBuilder.DropIndex(
			name: "ix_role_normalized_name_tenant_id",
			table: "role");

		migrationBuilder.DropIndex(
			name: "ix_role_tenant_id",
			table: "role");

		migrationBuilder.DropIndex(
			name: "RoleNameIndex",
			table: "role");

		migrationBuilder.DropColumn(
			name: "code",
			table: "role");

		migrationBuilder.DropColumn(
			name: "is_system",
			table: "role");

		migrationBuilder.DropColumn(
			name: "tenant_id",
			table: "role");

		migrationBuilder.CreateIndex(
			name: "RoleNameIndex",
			table: "role",
			column: "normalized_name",
			unique: true);

		migrationBuilder.Sql(
			"DELETE FROM permission WHERE name = 'tenant_read' OR name = 'tenant_manage'");

	}
}
