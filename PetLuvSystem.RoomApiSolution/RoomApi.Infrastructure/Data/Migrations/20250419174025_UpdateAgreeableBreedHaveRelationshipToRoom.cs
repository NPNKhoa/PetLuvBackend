using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAgreeableBreedHaveRelationshipToRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgreeableBreeds_RoomTypes_RoomTypeId",
                table: "AgreeableBreeds");

            migrationBuilder.RenameColumn(
                name: "RoomTypeId",
                table: "AgreeableBreeds",
                newName: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgreeableBreeds_Rooms_RoomId",
                table: "AgreeableBreeds",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "RoomId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AgreeableBreeds_Rooms_RoomId",
                table: "AgreeableBreeds");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "AgreeableBreeds",
                newName: "RoomTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AgreeableBreeds_RoomTypes_RoomTypeId",
                table: "AgreeableBreeds",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "RoomTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
