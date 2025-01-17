using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServiceApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVariantEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("5335e647-9fd3-4a3e-b816-c9c4259a3af5"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("61fc67c9-c5e6-4da2-b2b7-5d14de0a87d1"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("e9393082-0602-41b8-8830-d4cdbcd56c87"));

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "WalkDogServiceVariants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "ServiceVariants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "ServiceComboVariants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "ServiceTypeId", "IsVisible", "ServiceTypeDesc", "ServiceTypeName" },
                values: new object[,]
                {
                    { new Guid("44b1b972-4190-4acc-86ed-57cfc8bfe201"), false, "Chúng tôi sẵn sàng trực tiếp đến nhà chăm sóc cho \"boss\" của bạn nếu bạn đang quá bận", "Dịch vụ spa tại nhà" },
                    { new Guid("9b233771-2a56-4f21-a04c-9ff2f931f28c"), false, "Nếu bạn đang quá bận để giúp cún cưng của mình giải tỏa năng lượng, hãy để chúng tôi giúp bạn làm điều đó", "Dịch vụ dắt chó đi dạo" },
                    { new Guid("bb9189a5-b65a-4bb8-979b-fcb69b1d2ead"), false, "Cung cấp các dịch vụ spa cho thú cưng trực tiếp tại cửa hàng", "Dịch vụ spa" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("44b1b972-4190-4acc-86ed-57cfc8bfe201"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("9b233771-2a56-4f21-a04c-9ff2f931f28c"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("bb9189a5-b65a-4bb8-979b-fcb69b1d2ead"));

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "WalkDogServiceVariants");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "ServiceVariants");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "ServiceComboVariants");

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "ServiceTypeId", "IsVisible", "ServiceTypeDesc", "ServiceTypeName" },
                values: new object[,]
                {
                    { new Guid("5335e647-9fd3-4a3e-b816-c9c4259a3af5"), false, "Chúng tôi sẵn sàng trực tiếp đến nhà chăm sóc cho \"boss\" của bạn nếu bạn đang quá bận", "Dịch vụ spa tại nhà" },
                    { new Guid("61fc67c9-c5e6-4da2-b2b7-5d14de0a87d1"), false, "Nếu bạn đang quá bận để giúp cún cưng của mình giải tỏa năng lượng, hãy để chúng tôi giúp bạn làm điều đó", "Dịch vụ dắt chó đi dạo" },
                    { new Guid("e9393082-0602-41b8-8830-d4cdbcd56c87"), false, "Cung cấp các dịch vụ spa cho thú cưng trực tiếp tại cửa hàng", "Dịch vụ spa" }
                });
        }
    }
}
