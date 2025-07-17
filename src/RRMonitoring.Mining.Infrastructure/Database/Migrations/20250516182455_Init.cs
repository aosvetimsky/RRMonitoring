using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RRMonitoring.Mining.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    external_id = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "coin",
                columns: table => new
                {
                    id = table.Column<byte>(type: "smallint", nullable: false),
                    ticker = table.Column<string>(type: "text", nullable: false),
                    external_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_coin", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pool",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    external_id = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pool", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "worker_status",
                columns: table => new
                {
                    id = table.Column<byte>(type: "smallint", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_worker_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pool_coin_address",
                columns: table => new
                {
                    pool_id = table.Column<Guid>(type: "uuid", nullable: false),
                    coin_id = table.Column<byte>(type: "smallint", nullable: false),
                    first_address = table.Column<string>(type: "text", nullable: true),
                    second_address = table.Column<string>(type: "text", nullable: true),
                    third_address = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pool_coin_address", x => new { x.pool_id, x.coin_id });
                    table.ForeignKey(
                        name: "fk_pool_coin_address_coin_coin_id",
                        column: x => x.coin_id,
                        principalTable: "coin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_pool_coin_address_pool_pool_id",
                        column: x => x.pool_id,
                        principalTable: "pool",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "worker",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: false),
                    client_id = table.Column<Guid>(type: "uuid", nullable: true),
                    status_id = table.Column<byte>(type: "smallint", nullable: false),
                    pool_id = table.Column<Guid>(type: "uuid", nullable: false),
                    coin_id = table.Column<byte>(type: "smallint", nullable: false),
                    external_id = table.Column<string>(type: "text", nullable: false),
                    observer_link = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_worker", x => x.id);
                    table.ForeignKey(
                        name: "fk_worker_client_client_id",
                        column: x => x.client_id,
                        principalTable: "client",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_worker_coin_coin_id",
                        column: x => x.coin_id,
                        principalTable: "coin",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_worker_pool_pool_id",
                        column: x => x.pool_id,
                        principalTable: "pool",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_worker_worker_status_status_id",
                        column: x => x.status_id,
                        principalTable: "worker_status",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "coin",
                columns: new[] { "id", "external_id", "name", "ticker" },
                values: new object[,]
                {
                    { (byte)1, "89bb611e-7a3c-4adf-a190-89ceaaa6b174", "Conflux", "CFX" },
                    { (byte)2, "728912a7-1a00-4abb-96c0-fba5d4123957", "EthereumPoW", "ETHW" },
                    { (byte)3, "049a7e90-f063-4532-acd7-6fc74ec1ec64", "EthereumFair", "ETHF" },
                    { (byte)4, "7f08ec67-64f4-40ce-b18f-3a659d6614a8", "Nervos Network", "CKB" },
                    { (byte)5, "7d760102-9094-4829-a65a-ba749bddf013", "Ergo", "ERG" },
                    { (byte)6, "2bfc15a8-a452-4c03-9f7a-80b8ed853415", "Handshake", "HNS" },
                    { (byte)7, "975bc446-5f0e-4349-b00a-4a1d5f7b51a3", "Qitmeer Network", "MEER" },
                    { (byte)8, "c440283b-04c4-4f7b-b622-debe5b867c95", "eCash", "XEC" },
                    { (byte)9, "f28930db-d005-43b3-8ffd-5b62b15785df", "Bitcoin", "BTC" },
                    { (byte)10, "223e7ec7-c749-4909-b56f-cf2bee5aaa85", "Ethereum Classic", "ETC" },
                    { (byte)11, "86961652-cb82-4042-b3ae-05131494535b", "Kaspa", "KAS" },
                    { (byte)12, "1326ccaa-5082-4304-9270-0ce4e81fb57d", "Dash", "DASH" },
                    { (byte)13, "d175f805-b52f-4b93-b568-174bc6fca57d", "Litecoin", "LTC" },
                    { (byte)14, "b5bd1ee1-748e-481f-b4bf-f621989113db", "Zcash", "ZEC" },
                    { (byte)15, "3c96d353-adf6-4690-8f93-f85aa173441e", "Horizen", "ZEN" },
                    { (byte)16, "8f80498f-54ef-4b9d-957a-3b6ba4a26df3", "Aleo", "ALEO" },
                    { (byte)17, "cff35eb9-ed0c-4c5f-8c3f-68454bba3343", "Ravencoin", "RVN" },
                    { (byte)18, "25f59887-83d4-463b-8ec3-18a42abad96a", "Kadena", "KDA" },
                    { (byte)19, "5651424e-5055-4989-bfc3-062e717d3bff", "ScPrime", "SCP" }
                });

            migrationBuilder.InsertData(
                table: "worker_status",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { (byte)1, "Unrecognized" },
                    { (byte)2, "Active" },
                    { (byte)3, "Disabled" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_client_external_id",
                table: "client",
                column: "external_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_coin_external_id",
                table: "coin",
                column: "external_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_coin_ticker",
                table: "coin",
                column: "ticker",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_pool_external_id",
                table: "pool",
                column: "external_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_pool_coin_address_coin_id",
                table: "pool_coin_address",
                column: "coin_id");

            migrationBuilder.CreateIndex(
                name: "ix_worker_client_id",
                table: "worker",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_worker_coin_id",
                table: "worker",
                column: "coin_id");

            migrationBuilder.CreateIndex(
                name: "ix_worker_external_id",
                table: "worker",
                column: "external_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_worker_name_pool_id_coin_id",
                table: "worker",
                columns: new[] { "name", "pool_id", "coin_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_worker_pool_id",
                table: "worker",
                column: "pool_id");

            migrationBuilder.CreateIndex(
                name: "ix_worker_status_id",
                table: "worker",
                column: "status_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pool_coin_address");

            migrationBuilder.DropTable(
                name: "worker");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropTable(
                name: "coin");

            migrationBuilder.DropTable(
                name: "pool");

            migrationBuilder.DropTable(
                name: "worker_status");
        }
    }
}
