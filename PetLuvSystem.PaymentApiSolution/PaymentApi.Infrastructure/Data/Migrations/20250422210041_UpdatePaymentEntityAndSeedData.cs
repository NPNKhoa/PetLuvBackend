using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PaymentApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePaymentEntityAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentMethod",
                keyColumn: "PaymentMethodId",
                keyValue: new Guid("357f52f1-c6c5-417e-af87-4dd869eb9aa3"));

            migrationBuilder.DeleteData(
                table: "PaymentMethod",
                keyColumn: "PaymentMethodId",
                keyValue: new Guid("3f599ae2-329c-4ea3-9a0f-17e923639181"));

            migrationBuilder.DeleteData(
                table: "PaymentMethod",
                keyColumn: "PaymentMethodId",
                keyValue: new Guid("975225aa-2c71-4fe7-91c2-4038ee7abcd0"));

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: new Guid("14cdd281-43ad-4c82-afdd-f73f70f6974a"));

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: new Guid("a89d9447-9c08-4c2c-8067-ee3cced8cf9b"));

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: new Guid("d1f7edff-afdc-4b05-9c2f-3020b068eb7c"));

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: new Guid("e03badbf-c5a3-4468-b84e-fa2f8bd3cc9e"));

            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "PaymentMethodId", "IsVisible", "PaymentMethodName" },
                values: new object[,]
                {
                    { new Guid("32b0eca9-54fa-4a0f-9a5f-eca0731961ef"), true, "Thanh toán qua VNPay" },
                    { new Guid("f82ccee9-d212-4571-9a47-d987138dc452"), true, "Thanh toán tại cửa hàng" }
                });

            migrationBuilder.InsertData(
                table: "PaymentStatus",
                columns: new[] { "PaymentStatusId", "IsVisible", "PaymentStatusName" },
                values: new object[,]
                {
                    { new Guid("1cc46efb-9607-4301-b972-9808b3ed7054"), true, "Đã đặt cọc" },
                    { new Guid("f50814ca-3f54-4778-bd6c-ce913cac8284"), true, "Chờ thanh toán" },
                    { new Guid("f6f4ad85-005b-40e0-a91f-c1a4ef811a32"), true, "Đã thanh toán" },
                    { new Guid("fae369e6-f297-4ed7-9e4d-4ec3d6208264"), true, "Thanh toán thất bại" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentMethod",
                keyColumn: "PaymentMethodId",
                keyValue: new Guid("32b0eca9-54fa-4a0f-9a5f-eca0731961ef"));

            migrationBuilder.DeleteData(
                table: "PaymentMethod",
                keyColumn: "PaymentMethodId",
                keyValue: new Guid("f82ccee9-d212-4571-9a47-d987138dc452"));

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: new Guid("1cc46efb-9607-4301-b972-9808b3ed7054"));

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: new Guid("f50814ca-3f54-4778-bd6c-ce913cac8284"));

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: new Guid("f6f4ad85-005b-40e0-a91f-c1a4ef811a32"));

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: new Guid("fae369e6-f297-4ed7-9e4d-4ec3d6208264"));

            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "PaymentMethodId", "IsVisible", "PaymentMethodName" },
                values: new object[,]
                {
                    { new Guid("357f52f1-c6c5-417e-af87-4dd869eb9aa3"), true, "Thanh toán khi nhận hàng" },
                    { new Guid("3f599ae2-329c-4ea3-9a0f-17e923639181"), true, "Thanh toán qua VNPay" },
                    { new Guid("975225aa-2c71-4fe7-91c2-4038ee7abcd0"), true, "Thanh toán tại cửa hàng" }
                });

            migrationBuilder.InsertData(
                table: "PaymentStatus",
                columns: new[] { "PaymentStatusId", "IsVisible", "PaymentStatusName" },
                values: new object[,]
                {
                    { new Guid("14cdd281-43ad-4c82-afdd-f73f70f6974a"), true, "Đã thanh toán" },
                    { new Guid("a89d9447-9c08-4c2c-8067-ee3cced8cf9b"), true, "Thanh toán thất bại" },
                    { new Guid("d1f7edff-afdc-4b05-9c2f-3020b068eb7c"), true, "Đã đặt cọc" },
                    { new Guid("e03badbf-c5a3-4468-b84e-fa2f8bd3cc9e"), true, "Chờ thanh toán" }
                });
        }
    }
}
