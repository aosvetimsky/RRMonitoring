using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RRMonitoring.Colocation.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "facility",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    power_capacity = table.Column<int>(type: "integer", nullable: false),
                    is_archived = table.Column<bool>(type: "boolean", nullable: false),
                    manager_id = table.Column<Guid>(type: "uuid", nullable: false),
                    deputy_manager_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subnet = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_facility", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "socket_type",
                columns: table => new
                {
                    id = table.Column<byte>(type: "smallint", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_socket_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "facility_technician",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    facility_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_facility_technician", x => new { x.user_id, x.facility_id });
                    table.ForeignKey(
                        name: "fk_facility_technician_facility_facility_id",
                        column: x => x.facility_id,
                        principalTable: "facility",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "container",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    facility_id = table.Column<Guid>(type: "uuid", nullable: false),
                    socket_type_id = table.Column<byte>(type: "smallint", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_container", x => x.id);
                    table.ForeignKey(
                        name: "fk_container_facility_facility_id",
                        column: x => x.facility_id,
                        principalTable: "facility",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_container_socket_type_socket_type_id",
                        column: x => x.socket_type_id,
                        principalTable: "socket_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rack",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    container_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    socket_type_id = table.Column<byte>(type: "smallint", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_rack", x => x.id);
                    table.ForeignKey(
                        name: "fk_rack_container_container_id",
                        column: x => x.container_id,
                        principalTable: "container",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_rack_socket_type_socket_type_id",
                        column: x => x.socket_type_id,
                        principalTable: "socket_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "shelf",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rack_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    socket_type_id = table.Column<byte>(type: "smallint", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shelf", x => x.id);
                    table.ForeignKey(
                        name: "fk_shelf_rack_rack_id",
                        column: x => x.rack_id,
                        principalTable: "rack",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_shelf_socket_type_socket_type_id",
                        column: x => x.socket_type_id,
                        principalTable: "socket_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "place",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    shelf_id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    socket_type_id = table.Column<byte>(type: "smallint", nullable: false),
                    equipment_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_place", x => x.id);
                    table.ForeignKey(
                        name: "fk_place_shelf_shelf_id",
                        column: x => x.shelf_id,
                        principalTable: "shelf",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_place_socket_type_socket_type_id",
                        column: x => x.socket_type_id,
                        principalTable: "socket_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "socket_type",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { (byte)1, "C13", "C13" },
                    { (byte)2, "C19", "C19" },
                    { (byte)3, "PDU", "PDU" },
                    { (byte)4, "EuroPDU", "EuroPDU" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_container_facility_id",
                table: "container",
                column: "facility_id");

            migrationBuilder.CreateIndex(
                name: "ix_container_socket_type_id",
                table: "container",
                column: "socket_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_facility_technician_facility_id",
                table: "facility_technician",
                column: "facility_id");

            migrationBuilder.CreateIndex(
                name: "ix_place_shelf_id",
                table: "place",
                column: "shelf_id");

            migrationBuilder.CreateIndex(
                name: "ix_place_socket_type_id",
                table: "place",
                column: "socket_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_rack_container_id",
                table: "rack",
                column: "container_id");

            migrationBuilder.CreateIndex(
                name: "ix_rack_socket_type_id",
                table: "rack",
                column: "socket_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_shelf_rack_id",
                table: "shelf",
                column: "rack_id");

            migrationBuilder.CreateIndex(
                name: "ix_shelf_socket_type_id",
                table: "shelf",
                column: "socket_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "facility_technician");

            migrationBuilder.DropTable(
                name: "place");

            migrationBuilder.DropTable(
                name: "shelf");

            migrationBuilder.DropTable(
                name: "rack");

            migrationBuilder.DropTable(
                name: "container");

            migrationBuilder.DropTable(
                name: "facility");

            migrationBuilder.DropTable(
                name: "socket_type");
        }
    }
}
