using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServiceApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPeriodToWalkDogVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("3f2c2956-993d-4445-a305-109f172b54fe"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("4c58986e-361c-48f9-b131-0a8458c5604c"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("5f255741-b98a-4d13-9581-47c55a7ae33a"));

            migrationBuilder.AddColumn<int>(
                name: "Period",
                table: "WalkDogServiceVariants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "ServiceTypeId", "IsVisible", "ServiceTypeDesc", "ServiceTypeName" },
                values: new object[,]
                {
                    { new Guid("14039a05-c713-4853-9331-4d4871cd78df"), false, "Chúng tôi sẵn sàng trực tiếp đến nhà chăm sóc cho \"boss\" của bạn nếu bạn đang quá bận", "Dịch vụ spa tại nhà" },
                    { new Guid("c74b751b-56cc-4f9e-92eb-d08032f7af3b"), false, "Cung cấp các dịch vụ spa cho thú cưng trực tiếp tại cửa hàng", "Dịch vụ spa" },
                    { new Guid("d734146d-3fa5-42cc-bb0b-621c186badb4"), false, "Nếu bạn đang quá bận để giúp cún cưng của mình giải tỏa năng lượng, hãy để chúng tôi giúp bạn làm điều đó", "Dịch vụ dắt chó đi dạo" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("14039a05-c713-4853-9331-4d4871cd78df"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("c74b751b-56cc-4f9e-92eb-d08032f7af3b"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("d734146d-3fa5-42cc-bb0b-621c186badb4"));

            migrationBuilder.DropColumn(
                name: "Period",
                table: "WalkDogServiceVariants");

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "ServiceTypeId", "IsVisible", "ServiceTypeDesc", "ServiceTypeName" },
                values: new object[,]
                {
                    { new Guid("3f2c2956-993d-4445-a305-109f172b54fe"), false, "Chúng tôi sẵn sàng trực tiếp đến nhà chăm sóc cho \"boss\" của bạn nếu bạn đang quá bận", "Dịch vụ spa tại nhà" },
                    { new Guid("4c58986e-361c-48f9-b131-0a8458c5604c"), false, "Cung cấp các dịch vụ spa cho thú cưng trực tiếp tại cửa hàng", "Dịch vụ spa" },
                    { new Guid("5f255741-b98a-4d13-9581-47c55a7ae33a"), false, "Nếu bạn đang quá bận để giúp cún cưng của mình giải tỏa năng lượng, hãy để chúng tôi giúp bạn làm điều đó", "Dịch vụ dắt chó đi dạo" }
                });
        }
    }
}
