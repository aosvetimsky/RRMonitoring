using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class MakePermissionNameRequiredAndUnique : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql(@"
                UPDATE public.permission AS p
                SET
                    name = substring('dup' || gen_random_uuid() || COALESCE(p.name, '') FOR 150)
                WHERE
                    p.name IS NULL
                    OR EXISTS (
                        SELECT *
                        FROM public.permission p2
                        WHERE p2.name = p.name AND p2.ctid > p.ctid
                    )");

		migrationBuilder.AlterColumn<string>(
			name: "name",
			table: "permission",
			nullable: false,
			oldNullable: true);

		migrationBuilder.CreateIndex(
			name: "ix_permission_name",
			table: "permission",
			column: "name",
			unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "ix_permission_name",
			table: "permission");

		migrationBuilder.AlterColumn<string>(
			name: "name",
			table: "permission",
			nullable: true);
	}
}
