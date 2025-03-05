using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateServiceAndServiceComboBookingDetailKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceBookingDetails",
                table: "ServiceBookingDetails");

            migrationBuilder.RenameColumn(
                name: "ServiceVariantId",
                table: "ServiceBookingDetails",
                newName: "BreedId");

            migrationBuilder.AddColumn<Guid>(
                name: "BreedId",
                table: "ServiceComboBookingDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PetWeightRange",
                table: "ServiceComboBookingDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "ServiceBookingDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "PetWeightRange",
                table: "ServiceBookingDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceBookingDetails",
                table: "ServiceBookingDetails",
                columns: new[] { "BookingId", "ServiceId", "BreedId", "PetWeightRange" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceBookingDetails",
                table: "ServiceBookingDetails");

            migrationBuilder.DropColumn(
                name: "BreedId",
                table: "ServiceComboBookingDetails");

            migrationBuilder.DropColumn(
                name: "PetWeightRange",
                table: "ServiceComboBookingDetails");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "ServiceBookingDetails");

            migrationBuilder.DropColumn(
                name: "PetWeightRange",
                table: "ServiceBookingDetails");

            migrationBuilder.RenameColumn(
                name: "BreedId",
                table: "ServiceBookingDetails",
                newName: "ServiceVariantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceBookingDetails",
                table: "ServiceBookingDetails",
                columns: new[] { "BookingId", "ServiceVariantId" });
        }
    }
}
