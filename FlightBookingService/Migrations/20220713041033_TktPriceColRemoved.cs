using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightBookingService.Migrations
{
    public partial class TktPriceColRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketCost",
                table: "FlightDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TicketCost",
                table: "FlightDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
