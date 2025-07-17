using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddProfileValueToIdentityResource : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    insert into public.identity_resource (id, enabled, name, required, emphasize,
                                                    show_in_discovery_document, created, non_editable)
                    values (default, true, 'profile', false, false, true, current_timestamp, true);
                END $$;
            ");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{

	}
}
