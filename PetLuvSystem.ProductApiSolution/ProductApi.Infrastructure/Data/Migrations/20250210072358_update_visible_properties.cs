using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_visible_properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Isvisible",
                table: "FoodVariants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Isvisible",
                table: "FoodSizes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "Foods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "FoodImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "FoodFlavors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isvisible",
                table: "FoodVariants");

            migrationBuilder.DropColumn(
                name: "Isvisible",
                table: "FoodSizes");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "FoodImages");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "FoodFlavors");
        }
    }
}
