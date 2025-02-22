using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApi.Infrastructure.Date.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIsVisibleField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_BookingStatuses_BookingStatusId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_BookingTypes_BookingTypeId",
                table: "Bookings");

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "BookingTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "BookingStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_BookingStatuses_BookingStatusId",
                table: "Bookings",
                column: "BookingStatusId",
                principalTable: "BookingStatuses",
                principalColumn: "BookingStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_BookingTypes_BookingTypeId",
                table: "Bookings",
                column: "BookingTypeId",
                principalTable: "BookingTypes",
                principalColumn: "BookingTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_BookingStatuses_BookingStatusId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_BookingTypes_BookingTypeId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "BookingTypes");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "BookingStatuses");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_BookingStatuses_BookingStatusId",
                table: "Bookings",
                column: "BookingStatusId",
                principalTable: "BookingStatuses",
                principalColumn: "BookingStatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_BookingTypes_BookingTypeId",
                table: "Bookings",
                column: "BookingTypeId",
                principalTable: "BookingTypes",
                principalColumn: "BookingTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
