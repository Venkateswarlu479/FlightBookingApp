using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightSearchService.Migrations
{
    public partial class SearchData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightDetails",
                columns: table => new
                {
                    SearchSeqNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightId = table.Column<long>(type: "bigint", nullable: false),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AirlineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartureTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReachTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduledDays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstrumentUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoOfBizClassSeats = table.Column<int>(type: "int", nullable: false),
                    NoOfNonBizClassSeats = table.Column<int>(type: "int", nullable: false),
                    TicketCost = table.Column<double>(type: "float", nullable: false),
                    NoOfRows = table.Column<int>(type: "int", nullable: false),
                    OptedForMeal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlightStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastChangedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightDetails", x => x.SearchSeqNo);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightDetails");
        }
    }
}
