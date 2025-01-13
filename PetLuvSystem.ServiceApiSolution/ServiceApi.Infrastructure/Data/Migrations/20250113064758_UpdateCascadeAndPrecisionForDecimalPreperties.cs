using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServiceApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCascadeAndPrecisionForDecimalPreperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceComboMappings_ServiceCombos_ServiceComboId",
                table: "ServiceComboMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceComboMappings_Services_ServiceId",
                table: "ServiceComboMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceComboPrices_ServiceCombos_ServiceComboId",
                table: "ServiceComboPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceImages_Services_ServiceId",
                table: "ServiceImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicePrices_Services_ServiceId",
                table: "ServicePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_WalkDogServicePrices_Services_ServiceId",
                table: "WalkDogServicePrices");

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
                    { new Guid("8fceba7d-2595-4cc3-b036-5062588c7591"), false, "Cung cấp các dịch vụ spa cho thú cưng trực tiếp tại cửa hàng", "Dịch vụ spa" },
                    { new Guid("b8f5c2d2-b651-4341-870b-a6acc7c5187a"), false, "Nếu bạn đang quá bận để giúp cún cưng của mình giải tỏa năng lượng, hãy để chúng tôi giúp bạn làm điều đó", "Dịch vụ dắt chó đi dạo" },
                    { new Guid("cc4de076-eaca-4d91-90fe-c14727a2513a"), false, "Chúng tôi sẵn sàng trực tiếp đến nhà chăm sóc cho \"boss\" của bạn nếu bạn đang quá bận", "Dịch vụ spa tại nhà" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceComboMappings_ServiceCombos_ServiceComboId",
                table: "ServiceComboMappings",
                column: "ServiceComboId",
                principalTable: "ServiceCombos",
                principalColumn: "ServiceComboId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceComboMappings_Services_ServiceId",
                table: "ServiceComboMappings",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceComboPrices_ServiceCombos_ServiceComboId",
                table: "ServiceComboPrices",
                column: "ServiceComboId",
                principalTable: "ServiceCombos",
                principalColumn: "ServiceComboId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceImages_Services_ServiceId",
                table: "ServiceImages",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePrices_Services_ServiceId",
                table: "ServicePrices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalkDogServicePrices_Services_ServiceId",
                table: "WalkDogServicePrices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceComboMappings_ServiceCombos_ServiceComboId",
                table: "ServiceComboMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceComboMappings_Services_ServiceId",
                table: "ServiceComboMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceComboPrices_ServiceCombos_ServiceComboId",
                table: "ServiceComboPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceImages_Services_ServiceId",
                table: "ServiceImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicePrices_Services_ServiceId",
                table: "ServicePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_WalkDogServicePrices_Services_ServiceId",
                table: "WalkDogServicePrices");

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("8fceba7d-2595-4cc3-b036-5062588c7591"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("b8f5c2d2-b651-4341-870b-a6acc7c5187a"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("cc4de076-eaca-4d91-90fe-c14727a2513a"));

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "ServiceTypeId", "IsVisible", "ServiceTypeDesc", "ServiceTypeName" },
                values: new object[,]
                {
                    { new Guid("930b4133-191e-450a-baeb-7f39edc77d35"), false, "Chúng tôi sẵn sàng trực tiếp đến nhà chăm sóc cho \"boss\" của bạn nếu bạn đang quá bận", "Dịch vụ spa tại nhà" },
                    { new Guid("ae0b4a9b-71d7-4aed-9378-7e471aff9b0d"), false, "Cung cấp các dịch vụ spa cho thú cưng trực tiếp tại cửa hàng", "Dịch vụ spa" },
                    { new Guid("b5acc1a4-e982-4c6b-b4d2-90aa091cfdc4"), false, "Nếu bạn đang quá bận để giúp cún cưng của mình giải tỏa năng lượng, hãy để chúng tôi giúp bạn làm điều đó", "Dịch vụ dắt chó đi dạo" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceComboMappings_ServiceCombos_ServiceComboId",
                table: "ServiceComboMappings",
                column: "ServiceComboId",
                principalTable: "ServiceCombos",
                principalColumn: "ServiceComboId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceComboMappings_Services_ServiceId",
                table: "ServiceComboMappings",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceComboPrices_ServiceCombos_ServiceComboId",
                table: "ServiceComboPrices",
                column: "ServiceComboId",
                principalTable: "ServiceCombos",
                principalColumn: "ServiceComboId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceImages_Services_ServiceId",
                table: "ServiceImages",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePrices_Services_ServiceId",
                table: "ServicePrices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WalkDogServicePrices_Services_ServiceId",
                table: "WalkDogServicePrices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
