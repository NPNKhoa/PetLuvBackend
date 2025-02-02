using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PetTypes",
                columns: table => new
                {
                    PetTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PetTypeDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetTypes", x => x.PetTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PetBreeds",
                columns: table => new
                {
                    BreedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BreedName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BreedDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IllustrationImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    PetTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetBreeds", x => x.BreedId);
                    table.ForeignKey(
                        name: "FK_PetBreeds_PetTypes_PetTypeId",
                        column: x => x.PetTypeId,
                        principalTable: "PetTypes",
                        principalColumn: "PetTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PetDateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PetGender = table.Column<bool>(type: "bit", nullable: false),
                    PetFurColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PetWeight = table.Column<double>(type: "float", nullable: false),
                    PetDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PetFamilyRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    ParentPetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BreedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.PetId);
                    table.ForeignKey(
                        name: "FK_Pets_PetBreeds_BreedId",
                        column: x => x.BreedId,
                        principalTable: "PetBreeds",
                        principalColumn: "BreedId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pets_Pets_ParentPetId",
                        column: x => x.ParentPetId,
                        principalTable: "Pets",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PetHealthBooks",
                columns: table => new
                {
                    HealthBookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetHealthBooks", x => x.HealthBookId);
                    table.ForeignKey(
                        name: "FK_PetHealthBooks_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetImages",
                columns: table => new
                {
                    PetImagePath = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetImages", x => x.PetImagePath);
                    table.ForeignKey(
                        name: "FK_PetImages_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetHealthBookDetails",
                columns: table => new
                {
                    HealthBookDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HealthBookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetHealthNote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreatmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreatmentDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TreatmentProof = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VetDegree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetHealthBookDetails", x => new { x.HealthBookDetailId, x.HealthBookId });
                    table.ForeignKey(
                        name: "FK_PetHealthBookDetails_PetHealthBooks_HealthBookId",
                        column: x => x.HealthBookId,
                        principalTable: "PetHealthBooks",
                        principalColumn: "HealthBookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetBreeds_PetTypeId",
                table: "PetBreeds",
                column: "PetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PetHealthBookDetails_HealthBookId",
                table: "PetHealthBookDetails",
                column: "HealthBookId");

            migrationBuilder.CreateIndex(
                name: "IX_PetHealthBooks_PetId",
                table: "PetHealthBooks",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_PetImages_PetId",
                table: "PetImages",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_BreedId",
                table: "Pets",
                column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_ParentPetId",
                table: "Pets",
                column: "ParentPetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetHealthBookDetails");

            migrationBuilder.DropTable(
                name: "PetImages");

            migrationBuilder.DropTable(
                name: "PetHealthBooks");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "PetBreeds");

            migrationBuilder.DropTable(
                name: "PetTypes");
        }
    }
}
