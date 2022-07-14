using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightBookingService.Migrations
{
    public partial class BookingServiceMgn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookingClass",
                table: "BookingDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingClass",
                table: "BookingDetails");
        }
    }
}
