using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RoomApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AgreeableBreeds",
                keyColumns: new[] { "BreedId", "RoomId" },
                keyValues: new object[] { new Guid("19e108c9-258d-496c-87bd-28e92ff022b4"), new Guid("1608a4a8-e48e-4c2f-a6d8-6506145376ae") });

            migrationBuilder.DeleteData(
                table: "AgreeableBreeds",
                keyColumns: new[] { "BreedId", "RoomId" },
                keyValues: new object[] { new Guid("08f48311-3666-4ca0-a754-7751eac84655"), new Guid("243b87f1-8480-4cdb-af2b-b94edddd329f") });

            migrationBuilder.DeleteData(
                table: "AgreeableBreeds",
                keyColumns: new[] { "BreedId", "RoomId" },
                keyValues: new object[] { new Guid("91f6cc07-981f-4550-a5b7-d5dd31208c93"), new Guid("975e9364-74b4-418e-aa81-f856a495fbad") });

            migrationBuilder.DeleteData(
                table: "AgreeableBreeds",
                keyColumns: new[] { "BreedId", "RoomId" },
                keyValues: new object[] { new Guid("dc362b1f-b299-4b53-8a12-8cbf1779143d"), new Guid("a641d788-cbfc-4b96-a2a0-478ed3b1e8fc") });

            migrationBuilder.DeleteData(
                table: "AgreeableBreeds",
                keyColumns: new[] { "BreedId", "RoomId" },
                keyValues: new object[] { new Guid("5dc05029-3aad-4322-bc4c-73dc14657d20"), new Guid("a8dceed0-bc17-4a1d-9417-5613bd4cd2b9") });

            migrationBuilder.DeleteData(
                table: "RoomAccessories",
                keyColumn: "RoomAccessoryId",
                keyValue: new Guid("9ba27911-72de-4158-8b88-a9656964d8a4"));

            migrationBuilder.DeleteData(
                table: "RoomAccessories",
                keyColumn: "RoomAccessoryId",
                keyValue: new Guid("e41fcad3-e6fe-4e46-a3bb-85e88aaf6364"));

            migrationBuilder.DeleteData(
                table: "RoomAccessories",
                keyColumn: "RoomAccessoryId",
                keyValue: new Guid("fcf243d9-0b2a-49ef-bc7f-295bd66c75ba"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: new Guid("1608a4a8-e48e-4c2f-a6d8-6506145376ae"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: new Guid("243b87f1-8480-4cdb-af2b-b94edddd329f"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: new Guid("975e9364-74b4-418e-aa81-f856a495fbad"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: new Guid("a8dceed0-bc17-4a1d-9417-5613bd4cd2b9"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "RoomTypeId",
                keyValue: new Guid("7c65e6a6-c0dc-4c5c-95e4-2b8cd11b830b"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "RoomTypeId",
                keyValue: new Guid("b2f5b04b-0052-4c75-b0ed-8ae6b1a80ab3"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "RoomTypeId",
                keyValue: new Guid("f980fb89-ac4b-469c-abc1-137fe2acf3fd"));

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "RoomImagePath",
                keyValue: "101.jpg",
                column: "RoomId",
                value: new Guid("dd027e4c-06d4-409d-8d3c-58d9d55261fe"));

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "RoomImagePath",
                keyValue: "202.jpg",
                column: "RoomId",
                value: new Guid("320b9a5c-9d10-4bb3-995c-0748afed26be"));

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "RoomImagePath",
                keyValue: "303.jpg",
                column: "RoomId",
                value: new Guid("fef78c40-202a-482c-a9fd-0cf9e5756bb6"));

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "RoomImagePath",
                keyValue: "404.jpg",
                column: "RoomId",
                value: new Guid("60bc2bbd-1ac1-48b5-a45e-47b0bbbbd054"));

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "RoomImagePath",
                keyValue: "505.jpg",
                column: "RoomId",
                value: new Guid("add075db-a9a0-4107-8dec-d4090b43a305"));

            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "RoomTypeId", "IsVisible", "RoomTypeDesc", "RoomTypeName" },
                values: new object[,]
                {
                    { new Guid("059bc604-cb72-46fa-b1a1-2540c041093a"), true, "Phòng rộng rãi, tiện nghi cao cấp", "Phòng VIP" },
                    { new Guid("2c5147ae-5d4a-40e3-a3a3-4c7028c2169b"), true, "Dành cho nhiều thú cưng cùng ở", "Phòng Gia Đình" },
                    { new Guid("54d7e43b-ebd8-428d-8df1-825be9141de8"), true, "Phòng cơ bản cho thú cưng nhỏ", "Phòng Tiêu Chuẩn" }
                });

            migrationBuilder.InsertData(
                table: "RoomAccessories",
                columns: new[] { "RoomAccessoryId", "IsVisible", "RoomAccessoryDesc", "RoomAccessoryImagePath", "RoomAccessoryName", "RoomTypeId", "ServiceId" },
                values: new object[,]
                {
                    { new Guid("315877c2-c057-4f35-80b0-1af37b84529d"), true, "Khay vệ sinh cho mèo", "khaycat.jpg", "Khay cát", new Guid("2c5147ae-5d4a-40e3-a3a3-4c7028c2169b"), new Guid("fb42c512-3dd6-4fce-8a97-a683522ad691") },
                    { new Guid("46d6e8c3-4c53-4b2a-80c6-0f9138b49e7d"), true, "Cho mèo chơi và tập thể dục", "cayleo.jpg", "Cây leo", new Guid("059bc604-cb72-46fa-b1a1-2540c041093a"), new Guid("3591729e-3199-4bce-95e4-c502b8907e8c") },
                    { new Guid("56ce2368-f739-465b-ba7d-7f81a98309c0"), true, "Quan sát thú cưng từ xa", "camera.jpg", "Camera", new Guid("059bc604-cb72-46fa-b1a1-2540c041093a"), new Guid("92432a57-5845-42a8-bdf2-c5c8c84c43a9") }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "IsVisible", "PricePerDay", "PricePerHour", "RoomDesc", "RoomName", "RoomTypeId" },
                values: new object[,]
                {
                    { new Guid("320b9a5c-9d10-4bb3-995c-0748afed26be"), true, 500000m, 80000m, "Phòng VIP cho mèo quý tộc", "Phòng 202", new Guid("059bc604-cb72-46fa-b1a1-2540c041093a") },
                    { new Guid("60bc2bbd-1ac1-48b5-a45e-47b0bbbbd054"), true, 600000m, 90000m, "Phòng nhỏ cho mèo", "Phòng 404", new Guid("54d7e43b-ebd8-428d-8df1-825be9141de8") },
                    { new Guid("dd027e4c-06d4-409d-8d3c-58d9d55261fe"), true, 300000m, 50000m, "Phòng nhỏ, phù hợp chó con", "Phòng 101", new Guid("54d7e43b-ebd8-428d-8df1-825be9141de8") },
                    { new Guid("fef78c40-202a-482c-a9fd-0cf9e5756bb6"), true, 450000m, 70000m, "Phòng gia đình, cho mèo", "Phòng 303", new Guid("2c5147ae-5d4a-40e3-a3a3-4c7028c2169b") }
                });

            migrationBuilder.InsertData(
                table: "AgreeableBreeds",
                columns: new[] { "BreedId", "RoomId" },
                values: new object[,]
                {
                    { new Guid("aaae8a7b-abd1-4169-0383-08dd6a9d0b8b"), new Guid("320b9a5c-9d10-4bb3-995c-0748afed26be") },
                    { new Guid("aaae8a7b-abd1-4169-0383-08dd6a9d0b8b"), new Guid("60bc2bbd-1ac1-48b5-a45e-47b0bbbbd054") },
                    { new Guid("f5614fe7-b506-4379-d3fa-08dd53047a20"), new Guid("60bc2bbd-1ac1-48b5-a45e-47b0bbbbd054") },
                    { new Guid("aaae8a7b-abd1-4169-0383-08dd6a9d0b8b"), new Guid("fef78c40-202a-482c-a9fd-0cf9e5756bb6") },
                    { new Guid("f5614fe7-b506-4379-d3fa-08dd53047a20"), new Guid("fef78c40-202a-482c-a9fd-0cf9e5756bb6") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AgreeableBreeds",
                keyColumns: new[] { "BreedId", "RoomId" },
                keyValues: new object[] { new Guid("aaae8a7b-abd1-4169-0383-08dd6a9d0b8b"), new Guid("320b9a5c-9d10-4bb3-995c-0748afed26be") });

            migrationBuilder.DeleteData(
                table: "AgreeableBreeds",
                keyColumns: new[] { "BreedId", "RoomId" },
                keyValues: new object[] { new Guid("aaae8a7b-abd1-4169-0383-08dd6a9d0b8b"), new Guid("60bc2bbd-1ac1-48b5-a45e-47b0bbbbd054") });

            migrationBuilder.DeleteData(
                table: "AgreeableBreeds",
                keyColumns: new[] { "BreedId", "RoomId" },
                keyValues: new object[] { new Guid("f5614fe7-b506-4379-d3fa-08dd53047a20"), new Guid("60bc2bbd-1ac1-48b5-a45e-47b0bbbbd054") });

            migrationBuilder.DeleteData(
                table: "AgreeableBreeds",
                keyColumns: new[] { "BreedId", "RoomId" },
                keyValues: new object[] { new Guid("aaae8a7b-abd1-4169-0383-08dd6a9d0b8b"), new Guid("fef78c40-202a-482c-a9fd-0cf9e5756bb6") });

            migrationBuilder.DeleteData(
                table: "AgreeableBreeds",
                keyColumns: new[] { "BreedId", "RoomId" },
                keyValues: new object[] { new Guid("f5614fe7-b506-4379-d3fa-08dd53047a20"), new Guid("fef78c40-202a-482c-a9fd-0cf9e5756bb6") });

            migrationBuilder.DeleteData(
                table: "RoomAccessories",
                keyColumn: "RoomAccessoryId",
                keyValue: new Guid("315877c2-c057-4f35-80b0-1af37b84529d"));

            migrationBuilder.DeleteData(
                table: "RoomAccessories",
                keyColumn: "RoomAccessoryId",
                keyValue: new Guid("46d6e8c3-4c53-4b2a-80c6-0f9138b49e7d"));

            migrationBuilder.DeleteData(
                table: "RoomAccessories",
                keyColumn: "RoomAccessoryId",
                keyValue: new Guid("56ce2368-f739-465b-ba7d-7f81a98309c0"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: new Guid("dd027e4c-06d4-409d-8d3c-58d9d55261fe"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: new Guid("320b9a5c-9d10-4bb3-995c-0748afed26be"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: new Guid("60bc2bbd-1ac1-48b5-a45e-47b0bbbbd054"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomId",
                keyValue: new Guid("fef78c40-202a-482c-a9fd-0cf9e5756bb6"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "RoomTypeId",
                keyValue: new Guid("059bc604-cb72-46fa-b1a1-2540c041093a"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "RoomTypeId",
                keyValue: new Guid("2c5147ae-5d4a-40e3-a3a3-4c7028c2169b"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "RoomTypeId",
                keyValue: new Guid("54d7e43b-ebd8-428d-8df1-825be9141de8"));

            migrationBuilder.InsertData(
                table: "AgreeableBreeds",
                columns: new[] { "BreedId", "RoomId" },
                values: new object[] { new Guid("dc362b1f-b299-4b53-8a12-8cbf1779143d"), new Guid("a641d788-cbfc-4b96-a2a0-478ed3b1e8fc") });

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "RoomImagePath",
                keyValue: "101.jpg",
                column: "RoomId",
                value: new Guid("243b87f1-8480-4cdb-af2b-b94edddd329f"));

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "RoomImagePath",
                keyValue: "202.jpg",
                column: "RoomId",
                value: new Guid("975e9364-74b4-418e-aa81-f856a495fbad"));

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "RoomImagePath",
                keyValue: "303.jpg",
                column: "RoomId",
                value: new Guid("a8dceed0-bc17-4a1d-9417-5613bd4cd2b9"));

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "RoomImagePath",
                keyValue: "404.jpg",
                column: "RoomId",
                value: new Guid("1608a4a8-e48e-4c2f-a6d8-6506145376ae"));

            migrationBuilder.UpdateData(
                table: "RoomImages",
                keyColumn: "RoomImagePath",
                keyValue: "505.jpg",
                column: "RoomId",
                value: new Guid("a641d788-cbfc-4b96-a2a0-478ed3b1e8fc"));

            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "RoomTypeId", "IsVisible", "RoomTypeDesc", "RoomTypeName" },
                values: new object[,]
                {
                    { new Guid("7c65e6a6-c0dc-4c5c-95e4-2b8cd11b830b"), true, "Dành cho nhiều thú cưng cùng ở", "Phòng Gia Đình" },
                    { new Guid("b2f5b04b-0052-4c75-b0ed-8ae6b1a80ab3"), true, "Phòng rộng rãi, tiện nghi cao cấp", "Phòng VIP" },
                    { new Guid("f980fb89-ac4b-469c-abc1-137fe2acf3fd"), true, "Phòng cơ bản cho thú cưng nhỏ", "Phòng Tiêu Chuẩn" }
                });

            migrationBuilder.InsertData(
                table: "RoomAccessories",
                columns: new[] { "RoomAccessoryId", "IsVisible", "RoomAccessoryDesc", "RoomAccessoryImagePath", "RoomAccessoryName", "RoomTypeId", "ServiceId" },
                values: new object[,]
                {
                    { new Guid("9ba27911-72de-4158-8b88-a9656964d8a4"), true, "Quan sát thú cưng từ xa", "camera.jpg", "Camera", new Guid("b2f5b04b-0052-4c75-b0ed-8ae6b1a80ab3"), new Guid("0692b07e-e8ad-4307-ae4d-8225b443e925") },
                    { new Guid("e41fcad3-e6fe-4e46-a3bb-85e88aaf6364"), true, "Cho mèo chơi và tập thể dục", "cayleo.jpg", "Cây leo", new Guid("b2f5b04b-0052-4c75-b0ed-8ae6b1a80ab3"), new Guid("3e08b9d0-4544-43d2-8b76-9b271554ba02") },
                    { new Guid("fcf243d9-0b2a-49ef-bc7f-295bd66c75ba"), true, "Khay vệ sinh cho mèo", "khaycat.jpg", "Khay cát", new Guid("7c65e6a6-c0dc-4c5c-95e4-2b8cd11b830b"), new Guid("f0f1195a-4448-48e7-b687-a61532baa985") }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomId", "IsVisible", "PricePerDay", "PricePerHour", "RoomDesc", "RoomName", "RoomTypeId" },
                values: new object[,]
                {
                    { new Guid("1608a4a8-e48e-4c2f-a6d8-6506145376ae"), true, 600000m, 90000m, "Phòng nhỏ cho mèo", "Phòng 404", new Guid("f980fb89-ac4b-469c-abc1-137fe2acf3fd") },
                    { new Guid("243b87f1-8480-4cdb-af2b-b94edddd329f"), true, 300000m, 50000m, "Phòng nhỏ, phù hợp chó con", "Phòng 101", new Guid("f980fb89-ac4b-469c-abc1-137fe2acf3fd") },
                    { new Guid("975e9364-74b4-418e-aa81-f856a495fbad"), true, 500000m, 80000m, "Phòng VIP cho mèo quý tộc", "Phòng 202", new Guid("b2f5b04b-0052-4c75-b0ed-8ae6b1a80ab3") },
                    { new Guid("a8dceed0-bc17-4a1d-9417-5613bd4cd2b9"), true, 450000m, 70000m, "Phòng gia đình có camera", "Phòng 303", new Guid("7c65e6a6-c0dc-4c5c-95e4-2b8cd11b830b") }
                });

            migrationBuilder.InsertData(
                table: "AgreeableBreeds",
                columns: new[] { "BreedId", "RoomId" },
                values: new object[,]
                {
                    { new Guid("19e108c9-258d-496c-87bd-28e92ff022b4"), new Guid("1608a4a8-e48e-4c2f-a6d8-6506145376ae") },
                    { new Guid("08f48311-3666-4ca0-a754-7751eac84655"), new Guid("243b87f1-8480-4cdb-af2b-b94edddd329f") },
                    { new Guid("91f6cc07-981f-4550-a5b7-d5dd31208c93"), new Guid("975e9364-74b4-418e-aa81-f856a495fbad") },
                    { new Guid("5dc05029-3aad-4322-bc4c-73dc14657d20"), new Guid("a8dceed0-bc17-4a1d-9417-5613bd4cd2b9") }
                });
        }
    }
}
