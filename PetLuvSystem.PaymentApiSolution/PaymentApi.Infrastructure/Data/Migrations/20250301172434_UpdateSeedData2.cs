using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                table: "PaymentMethod",
                columns: new[] { "PaymentMethodId", "IsVisible", "PaymentMethodName" },
                values: new object[] { new Guid("dc35c302-fd44-43e4-a007-b16e8a6234ef"), true, "Thanh toán qua VNPay" });

            migrationBuilder.InsertData(
                table: "PaymentStatus",
                columns: new[] { "PaymentStatusId", "IsVisible", "PaymentStatusName" },
                values: new object[] { new Guid("87fb356a-980c-49bd-b21e-539ddc0746fd"), true, "Chờ thanh toán" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PaymentMethod",
                keyColumn: "PaymentMethodId",
                keyValue: new Guid("dc35c302-fd44-43e4-a007-b16e8a6234ef"));

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: new Guid("87fb356a-980c-49bd-b21e-539ddc0746fd"));

            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "PaymentMethodId", "IsVisible", "PaymentMethodName" },
                values: new object[] { new Guid("52789035-c5c1-420f-b43d-dc39fb150333"), true, "Chờ thanh toán" });

            migrationBuilder.InsertData(
                table: "PaymentStatus",
                columns: new[] { "PaymentStatusId", "IsVisible", "PaymentStatusName" },
                values: new object[] { new Guid("71b8eb1d-f84a-4f3a-9fd1-3e70ab9470f6"), true, "Thanh toán qua VNPay" });
        }
    }
}
