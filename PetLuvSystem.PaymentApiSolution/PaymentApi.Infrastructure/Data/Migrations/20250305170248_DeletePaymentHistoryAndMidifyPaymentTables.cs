using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PaymentApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeletePaymentHistoryAndMidifyPaymentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentHistory");

            migrationBuilder.DeleteData(
                table: "PaymentMethod",
                keyColumn: "PaymentMethodId",
                keyValue: new Guid("dc35c302-fd44-43e4-a007-b16e8a6234ef"));

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: new Guid("87fb356a-980c-49bd-b21e-539ddc0746fd"));

            migrationBuilder.DropColumn(
                name: "ResponseData",
                table: "Payment");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Payment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedTime",
                table: "Payment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "Payment");

            migrationBuilder.AddColumn<string>(
                name: "ResponseData",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentHistory",
                columns: table => new
                {
                    PaymentHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentHistory", x => x.PaymentHistoryId);
                    table.ForeignKey(
                        name: "FK_PaymentHistory_PaymentStatus_PaymentStatusId",
                        column: x => x.PaymentStatusId,
                        principalTable: "PaymentStatus",
                        principalColumn: "PaymentStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentHistory_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "PaymentMethodId", "IsVisible", "PaymentMethodName" },
                values: new object[] { new Guid("dc35c302-fd44-43e4-a007-b16e8a6234ef"), true, "Thanh toán qua VNPay" });

            migrationBuilder.InsertData(
                table: "PaymentStatus",
                columns: new[] { "PaymentStatusId", "IsVisible", "PaymentStatusName" },
                values: new object[] { new Guid("87fb356a-980c-49bd-b21e-539ddc0746fd"), true, "Chờ thanh toán" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_PaymentId",
                table: "PaymentHistory",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentHistory_PaymentStatusId",
                table: "PaymentHistory",
                column: "PaymentStatusId");
        }
    }
}
