using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRMonitoring.Identity.Infrastructure.Database.Migrations;

public partial class AddOfflineAccessScope : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.Sql(@"
                insert into public.api_scope
                    (id, enabled, name, display_name, description, required, emphasize, show_in_discovery_document)
                values (default, true, 'offline_access', 'Offline access scope', null, true, false, true);");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{

	}
}
