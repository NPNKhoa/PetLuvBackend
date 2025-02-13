using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class modify_some_properties_and_relationship_in_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetHealthBooks_Pets_PetId",
                table: "PetHealthBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Pets_ParentPetId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_PetHealthBooks_PetId",
                table: "PetHealthBooks");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "PetHealthBooks");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "PetHealthBooks");

            migrationBuilder.RenameColumn(
                name: "ParentPetId",
                table: "Pets",
                newName: "MotherId");

            migrationBuilder.RenameIndex(
                name: "IX_Pets_ParentPetId",
                table: "Pets",
                newName: "IX_Pets_MotherId");

            migrationBuilder.RenameColumn(
                name: "HealthBookId",
                table: "PetHealthBooks",
                newName: "PetHealthBookId");

            migrationBuilder.AlterColumn<string>(
                name: "PetFurColor",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "FatherId",
                table: "Pets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_FatherId",
                table: "Pets",
                column: "FatherId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetHealthBooks_Pets_PetHealthBookId",
                table: "PetHealthBooks",
                column: "PetHealthBookId",
                principalTable: "Pets",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Pets_FatherId",
                table: "Pets",
                column: "FatherId",
                principalTable: "Pets",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Pets_MotherId",
                table: "Pets",
                column: "MotherId",
                principalTable: "Pets",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetHealthBooks_Pets_PetHealthBookId",
                table: "PetHealthBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Pets_FatherId",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Pets_MotherId",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_FatherId",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "FatherId",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "MotherId",
                table: "Pets",
                newName: "ParentPetId");

            migrationBuilder.RenameIndex(
                name: "IX_Pets_MotherId",
                table: "Pets",
                newName: "IX_Pets_ParentPetId");

            migrationBuilder.RenameColumn(
                name: "PetHealthBookId",
                table: "PetHealthBooks",
                newName: "HealthBookId");

            migrationBuilder.AlterColumn<string>(
                name: "PetFurColor",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "PetHealthBooks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "PetId",
                table: "PetHealthBooks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetHealthBooks_PetId",
                table: "PetHealthBooks",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_PetHealthBooks_Pets_PetId",
                table: "PetHealthBooks",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Pets_ParentPetId",
                table: "Pets",
                column: "ParentPetId",
                principalTable: "Pets",
                principalColumn: "PetId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
