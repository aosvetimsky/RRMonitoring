using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RRMonitoring.Equipment.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "equipment_mode",
                columns: table => new
                {
                    id = table.Column<byte>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipment_mode", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "equipment_status",
                columns: table => new
                {
                    id = table.Column<byte>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipment_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "firmware",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    version = table.Column<string>(type: "text", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    origin_file_name = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_firmware", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hashrate_unit",
                columns: table => new
                {
                    id = table.Column<byte>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hashrate_unit", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "manufacturer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_manufacturer", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "scan_status",
                columns: table => new
                {
                    id = table.Column<byte>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_scan_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "equipment_model",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    manufacturer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    hashrate_unit_id = table.Column<byte>(type: "smallint", nullable: false),
                    nominal_hashrate = table.Column<decimal>(type: "numeric", nullable: false),
                    nominal_power = table.Column<int>(type: "integer", nullable: false),
                    max_mother_board_temperature = table.Column<int>(type: "integer", nullable: false),
                    max_processor_temperature = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipment_model", x => x.id);
                    table.ForeignKey(
                        name: "fk_equipment_model_hashrate_unit_hashrate_unit_id",
                        column: x => x.hashrate_unit_id,
                        principalTable: "hashrate_unit",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_equipment_model_manufacturer_manufacturer_id",
                        column: x => x.manufacturer_id,
                        principalTable: "manufacturer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "scan",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    ip_range_definition = table.Column<string>(type: "text", nullable: false),
                    status_id = table.Column<byte>(type: "smallint", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_scan", x => x.id);
                    table.ForeignKey(
                        name: "fk_scan_scan_status_status_id",
                        column: x => x.status_id,
                        principalTable: "scan_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "equipment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    model_id = table.Column<Guid>(type: "uuid", nullable: false),
                    serial_number = table.Column<string>(type: "text", nullable: false),
                    ip_address = table.Column<string>(type: "text", nullable: true),
                    mac_address = table.Column<string>(type: "text", nullable: false),
                    current_hashrate = table.Column<decimal>(type: "numeric", nullable: false),
                    current_power = table.Column<decimal>(type: "numeric", nullable: false),
                    status_id = table.Column<byte>(type: "smallint", nullable: false),
                    mode_id = table.Column<byte>(type: "smallint", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipment", x => x.id);
                    table.ForeignKey(
                        name: "fk_equipment_equipment_mode_mode_id",
                        column: x => x.mode_id,
                        principalTable: "equipment_mode",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_equipment_equipment_model_model_id",
                        column: x => x.model_id,
                        principalTable: "equipment_model",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_equipment_equipment_status_status_id",
                        column: x => x.status_id,
                        principalTable: "equipment_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "equipment_model_coin",
                columns: table => new
                {
                    equipment_model_id = table.Column<Guid>(type: "uuid", nullable: false),
                    coin_id = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipment_model_coin", x => new { x.equipment_model_id, x.coin_id });
                    table.ForeignKey(
                        name: "fk_equipment_model_coin_equipment_model_equipment_model_id",
                        column: x => x.equipment_model_id,
                        principalTable: "equipment_model",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "firmware_equipment_model",
                columns: table => new
                {
                    firmware_id = table.Column<Guid>(type: "uuid", nullable: false),
                    equipment_model_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_firmware_equipment_model", x => new { x.firmware_id, x.equipment_model_id });
                    table.ForeignKey(
                        name: "fk_firmware_equipment_model_equipment_model_equipment_model_id",
                        column: x => x.equipment_model_id,
                        principalTable: "equipment_model",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_firmware_equipment_model_firmware_firmware_id",
                        column: x => x.firmware_id,
                        principalTable: "firmware",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scan_result",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    scan_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ip_address = table.Column<string>(type: "text", nullable: false),
                    mac_address = table.Column<string>(type: "text", nullable: false),
                    detected_model = table.Column<string>(type: "text", nullable: false),
                    firmware_version = table.Column<string>(type: "text", nullable: true),
                    equipment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_new_equipment = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_scan_result", x => x.id);
                    table.ForeignKey(
                        name: "fk_scan_result_equipment_equipment_id",
                        column: x => x.equipment_id,
                        principalTable: "equipment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_scan_result_scan_scan_id",
                        column: x => x.scan_id,
                        principalTable: "scan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "hashrate_unit",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { (byte)1, "H/s" },
                    { (byte)2, "K/Sol" },
                    { (byte)3, "MH/s" },
                    { (byte)4, "GH/s" },
                    { (byte)5, "TH/s" },
                    { (byte)6, "PH/s" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_equipment_ip_address",
                table: "equipment",
                column: "ip_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_equipment_mode_id",
                table: "equipment",
                column: "mode_id");

            migrationBuilder.CreateIndex(
                name: "ix_equipment_model_id",
                table: "equipment",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "ix_equipment_status_id",
                table: "equipment",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_equipment_model_hashrate_unit_id",
                table: "equipment_model",
                column: "hashrate_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_equipment_model_manufacturer_id",
                table: "equipment_model",
                column: "manufacturer_id");

            migrationBuilder.CreateIndex(
                name: "ix_firmware_equipment_model_equipment_model_id",
                table: "firmware_equipment_model",
                column: "equipment_model_id");

            migrationBuilder.CreateIndex(
                name: "ix_scan_status_id",
                table: "scan",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_scan_result_equipment_id",
                table: "scan_result",
                column: "equipment_id");

            migrationBuilder.CreateIndex(
                name: "ix_scan_result_scan_id",
                table: "scan_result",
                column: "scan_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "equipment_model_coin");

            migrationBuilder.DropTable(
                name: "firmware_equipment_model");

            migrationBuilder.DropTable(
                name: "scan_result");

            migrationBuilder.DropTable(
                name: "firmware");

            migrationBuilder.DropTable(
                name: "equipment");

            migrationBuilder.DropTable(
                name: "scan");

            migrationBuilder.DropTable(
                name: "equipment_mode");

            migrationBuilder.DropTable(
                name: "equipment_model");

            migrationBuilder.DropTable(
                name: "equipment_status");

            migrationBuilder.DropTable(
                name: "scan_status");

            migrationBuilder.DropTable(
                name: "hashrate_unit");

            migrationBuilder.DropTable(
                name: "manufacturer");
        }
    }
}
