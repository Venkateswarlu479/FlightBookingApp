using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightBookingService.Migrations
{
    public partial class FlightTableModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BizClassTicketPrice",
                table: "FlightDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NonBizClassTicketPrice",
                table: "FlightDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BizClassTicketPrice",
                table: "FlightDetails");

            migrationBuilder.DropColumn(
                name: "NonBizClassTicketPrice",
                table: "FlightDetails");
        }
    }
}
