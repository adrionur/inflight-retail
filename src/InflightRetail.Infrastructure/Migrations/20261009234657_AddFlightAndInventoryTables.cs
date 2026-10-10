using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InflightRetail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFlightAndInventoryTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "flights",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FlightNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Origin = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Destination = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    DepartureTimeUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AircraftType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyRates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FlightId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetCurrency = table.Column<string>(type: "text", nullable: false),
                    RateAgainstBase = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyRates_flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "flight_menu_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FlightId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpliftQuantity = table.Column<int>(type: "integer", nullable: false),
                    RemainingQuantity = table.Column<int>(type: "integer", nullable: false),
                    OverrideBasePrice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flight_menu_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_flight_menu_items_flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_flight_menu_items_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRates_FlightId",
                table: "CurrencyRates",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_flight_menu_items_FlightId_ProductId",
                table: "flight_menu_items",
                columns: new[] { "FlightId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_flight_menu_items_ProductId",
                table: "flight_menu_items",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyRates");

            migrationBuilder.DropTable(
                name: "flight_menu_items");

            migrationBuilder.DropTable(
                name: "flights");
        }
    }
}
