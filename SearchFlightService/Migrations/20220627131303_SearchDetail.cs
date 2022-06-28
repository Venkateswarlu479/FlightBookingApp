using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightSearchService.Migrations
{
    public partial class SearchDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightDetails",
                columns: table => new
                {
                    FlightId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlightName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TripType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JourneyDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketPrice = table.Column<double>(type: "float", nullable: false),
                    FlightStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastChangedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightDetails", x => x.FlightId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightDetails");
        }
    }
}
