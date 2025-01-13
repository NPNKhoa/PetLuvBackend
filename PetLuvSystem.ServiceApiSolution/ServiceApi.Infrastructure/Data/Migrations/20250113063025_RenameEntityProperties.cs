using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServiceApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameEntityProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("7dcc1caa-f5f0-444c-a1a9-bce0d8dcd1be"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("bf8b21d0-d5d4-4c97-9caf-c7f8fc597714"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("cfd50999-b881-4b08-b69c-5509d92c8ce5"));

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "ServiceTypeId", "IsVisible", "ServiceTypeDesc", "ServiceTypeName" },
                values: new object[,]
                {
                    { new Guid("930b4133-191e-450a-baeb-7f39edc77d35"), false, "Chúng tôi sẵn sàng trực tiếp đến nhà chăm sóc cho \"boss\" của bạn nếu bạn đang quá bận", "Dịch vụ spa tại nhà" },
                    { new Guid("ae0b4a9b-71d7-4aed-9378-7e471aff9b0d"), false, "Cung cấp các dịch vụ spa cho thú cưng trực tiếp tại cửa hàng", "Dịch vụ spa" },
                    { new Guid("b5acc1a4-e982-4c6b-b4d2-90aa091cfdc4"), false, "Nếu bạn đang quá bận để giúp cún cưng của mình giải tỏa năng lượng, hãy để chúng tôi giúp bạn làm điều đó", "Dịch vụ dắt chó đi dạo" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("930b4133-191e-450a-baeb-7f39edc77d35"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("ae0b4a9b-71d7-4aed-9378-7e471aff9b0d"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("b5acc1a4-e982-4c6b-b4d2-90aa091cfdc4"));

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "ServiceTypeId", "IsVisible", "ServiceTypeDesc", "ServiceTypeName" },
                values: new object[,]
                {
                    { new Guid("7dcc1caa-f5f0-444c-a1a9-bce0d8dcd1be"), false, "Chúng tôi sẵn sàng trực tiếp đến nhà chăm sóc cho \"boss\" của bạn nếu bạn đang quá bận", "Dịch vụ spa tại nhà" },
                    { new Guid("bf8b21d0-d5d4-4c97-9caf-c7f8fc597714"), false, "Cung cấp các dịch vụ spa cho thú cưng trực tiếp tại cửa hàng", "Dịch vụ spa" },
                    { new Guid("cfd50999-b881-4b08-b69c-5509d92c8ce5"), false, "Nếu bạn đang quá bận để giúp cún cưng của mình giải tỏa năng lượng, hãy để chúng tôi giúp bạn làm điều đó", "Dịch vụ dắt chó đi dạo" }
                });
        }
    }
}
