using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BookingStatuses",
                columns: new[] { "BookingStatusId", "BookingStatusName", "IsVisible" },
                values: new object[,]
                {
                    { new Guid("10dac2d7-1cb1-4d82-839e-343072429624"), "Đã hủy", true },
                    { new Guid("632898b3-d675-4b4f-bf49-cf8c629ddb8c"), "Đã xác nhận", true },
                    { new Guid("888d34ef-e815-42f3-bccf-21f51981b1d3"), "Đang xử lý", true },
                    { new Guid("8d314818-a01d-4d60-8b26-dc7d047693ae"), "Đã hoàn thành", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookingStatuses",
                keyColumn: "BookingStatusId",
                keyValue: new Guid("10dac2d7-1cb1-4d82-839e-343072429624"));

            migrationBuilder.DeleteData(
                table: "BookingStatuses",
                keyColumn: "BookingStatusId",
                keyValue: new Guid("632898b3-d675-4b4f-bf49-cf8c629ddb8c"));

            migrationBuilder.DeleteData(
                table: "BookingStatuses",
                keyColumn: "BookingStatusId",
                keyValue: new Guid("888d34ef-e815-42f3-bccf-21f51981b1d3"));

            migrationBuilder.DeleteData(
                table: "BookingStatuses",
                keyColumn: "BookingStatusId",
                keyValue: new Guid("8d314818-a01d-4d60-8b26-dc7d047693ae"));
        }
    }
}
