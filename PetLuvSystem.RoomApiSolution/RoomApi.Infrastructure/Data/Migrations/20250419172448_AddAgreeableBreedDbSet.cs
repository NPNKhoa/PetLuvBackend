using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAgreeableBreedDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgreeableBreed_RoomTypes_RoomTypeId",
                table: "AgreeableBreed");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgreeableBreed",
                table: "AgreeableBreed");

            migrationBuilder.RenameTable(
                name: "AgreeableBreed",
                newName: "AgreeableBreeds");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgreeableBreeds",
                table: "AgreeableBreeds",
                columns: new[] { "RoomTypeId", "BreedId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AgreeableBreeds_RoomTypes_RoomTypeId",
                table: "AgreeableBreeds",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "RoomTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgreeableBreeds_RoomTypes_RoomTypeId",
                table: "AgreeableBreeds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AgreeableBreeds",
                table: "AgreeableBreeds");

            migrationBuilder.RenameTable(
                name: "AgreeableBreeds",
                newName: "AgreeableBreed");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AgreeableBreed",
                table: "AgreeableBreed",
                columns: new[] { "RoomTypeId", "BreedId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AgreeableBreed_RoomTypes_RoomTypeId",
                table: "AgreeableBreed",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "RoomTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
