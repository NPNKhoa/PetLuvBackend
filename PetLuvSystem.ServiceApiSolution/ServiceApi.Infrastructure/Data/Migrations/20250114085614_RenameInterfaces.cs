using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServiceApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameInterfaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceComboPrices_ServiceCombos_ServiceComboId",
                table: "ServiceComboPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicePrices_Services_ServiceId",
                table: "ServicePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_WalkDogServicePrices_Services_ServiceId",
                table: "WalkDogServicePrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WalkDogServicePrices",
                table: "WalkDogServicePrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServicePrices",
                table: "ServicePrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceComboPrices",
                table: "ServiceComboPrices");

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

            migrationBuilder.RenameTable(
                name: "WalkDogServicePrices",
                newName: "WalkDogServiceVariants");

            migrationBuilder.RenameTable(
                name: "ServicePrices",
                newName: "ServiceVariants");

            migrationBuilder.RenameTable(
                name: "ServiceComboPrices",
                newName: "ServiceComboVariants");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WalkDogServiceVariants",
                table: "WalkDogServiceVariants",
                column: "ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceVariants",
                table: "ServiceVariants",
                columns: new[] { "ServiceId", "BreedId", "PetWeightRange" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceComboVariants",
                table: "ServiceComboVariants",
                columns: new[] { "ServiceComboId", "BreedId", "WeightRange" });

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "ServiceTypeId", "IsVisible", "ServiceTypeDesc", "ServiceTypeName" },
                values: new object[,]
                {
                    { new Guid("3a7b34b9-9492-4994-866a-9ab504a9fddb"), false, "Cung cấp các dịch vụ spa cho thú cưng trực tiếp tại cửa hàng", "Dịch vụ spa" },
                    { new Guid("5ef35d81-3523-4c90-ab7c-b4a3c594acf7"), false, "Nếu bạn đang quá bận để giúp cún cưng của mình giải tỏa năng lượng, hãy để chúng tôi giúp bạn làm điều đó", "Dịch vụ dắt chó đi dạo" },
                    { new Guid("89a307fb-4eb9-439b-b4a2-c763382d8243"), false, "Chúng tôi sẵn sàng trực tiếp đến nhà chăm sóc cho \"boss\" của bạn nếu bạn đang quá bận", "Dịch vụ spa tại nhà" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceComboVariants_ServiceCombos_ServiceComboId",
                table: "ServiceComboVariants",
                column: "ServiceComboId",
                principalTable: "ServiceCombos",
                principalColumn: "ServiceComboId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceVariants_Services_ServiceId",
                table: "ServiceVariants",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalkDogServiceVariants_Services_ServiceId",
                table: "WalkDogServiceVariants",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceComboVariants_ServiceCombos_ServiceComboId",
                table: "ServiceComboVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceVariants_Services_ServiceId",
                table: "ServiceVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_WalkDogServiceVariants_Services_ServiceId",
                table: "WalkDogServiceVariants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WalkDogServiceVariants",
                table: "WalkDogServiceVariants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceVariants",
                table: "ServiceVariants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceComboVariants",
                table: "ServiceComboVariants");

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("3a7b34b9-9492-4994-866a-9ab504a9fddb"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("5ef35d81-3523-4c90-ab7c-b4a3c594acf7"));

            migrationBuilder.DeleteData(
                table: "ServiceTypes",
                keyColumn: "ServiceTypeId",
                keyValue: new Guid("89a307fb-4eb9-439b-b4a2-c763382d8243"));

            migrationBuilder.RenameTable(
                name: "WalkDogServiceVariants",
                newName: "WalkDogServicePrices");

            migrationBuilder.RenameTable(
                name: "ServiceVariants",
                newName: "ServicePrices");

            migrationBuilder.RenameTable(
                name: "ServiceComboVariants",
                newName: "ServiceComboPrices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WalkDogServicePrices",
                table: "WalkDogServicePrices",
                column: "ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServicePrices",
                table: "ServicePrices",
                columns: new[] { "ServiceId", "BreedId", "PetWeightRange" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceComboPrices",
                table: "ServiceComboPrices",
                columns: new[] { "ServiceComboId", "BreedId", "WeightRange" });

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
                name: "FK_ServiceComboPrices_ServiceCombos_ServiceComboId",
                table: "ServiceComboPrices",
                column: "ServiceComboId",
                principalTable: "ServiceCombos",
                principalColumn: "ServiceComboId",
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
    }
}
