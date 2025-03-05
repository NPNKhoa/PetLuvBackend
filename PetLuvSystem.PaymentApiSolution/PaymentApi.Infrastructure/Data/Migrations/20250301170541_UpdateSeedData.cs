using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PaymentApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: new Guid("06b04b04-d94d-4cd6-930a-2d6bf125d6ca"));

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: new Guid("aca8cf7e-7c5e-4a7c-8a24-07d5c9bdef4b"));

            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "PaymentMethodId", "IsVisible", "PaymentMethodName" },
                values: new object[] { new Guid("52789035-c5c1-420f-b43d-dc39fb150333"), true, "Chờ thanh toán" });

            migrationBuilder.InsertData(
                table: "PaymentStatus",
                columns: new[] { "PaymentStatusId", "IsVisible", "PaymentStatusName" },
                values: new object[] { new Guid("71b8eb1d-f84a-4f3a-9fd1-3e70ab9470f6"), true, "Thanh toán qua VNPay" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentMethod",
                keyColumn: "PaymentMethodId",
                keyValue: new Guid("52789035-c5c1-420f-b43d-dc39fb150333"));

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: new Guid("71b8eb1d-f84a-4f3a-9fd1-3e70ab9470f6"));

            migrationBuilder.InsertData(
                table: "PaymentStatus",
                columns: new[] { "PaymentStatusId", "IsVisible", "PaymentStatusName" },
                values: new object[,]
                {
                    { new Guid("06b04b04-d94d-4cd6-930a-2d6bf125d6ca"), true, "Chờ thanh toán" },
                    { new Guid("aca8cf7e-7c5e-4a7c-8a24-07d5c9bdef4b"), true, "Thanh toán qua VNPay" }
                });
        }
    }
}
