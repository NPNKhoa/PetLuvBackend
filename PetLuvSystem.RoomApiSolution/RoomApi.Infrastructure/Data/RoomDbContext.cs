using Microsoft.EntityFrameworkCore;
using RoomApi.Domain.Entities;

namespace RoomApi.Infrastructure.Data
{
    public class RoomDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomAccessory> RoomAccessories { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<AgreeableBreed> AgreeableBreeds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Room Type
            modelBuilder.Entity<RoomType>().HasKey(rt => rt.RoomTypeId);
            modelBuilder.Entity<RoomType>().Property(rt => rt.RoomTypeName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<RoomType>().Property(rt => rt.RoomTypeDesc).HasMaxLength(500);

            modelBuilder.Entity<RoomType>()
               .HasMany(rt => rt.Rooms)
               .WithOne(r => r.RoomType)
               .HasForeignKey(r => r.RoomTypeId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RoomType>()
               .HasMany(rt => rt.RoomAccessories)
               .WithOne(ra => ra.RoomType)
               .HasForeignKey(ra => ra.RoomTypeId)
               .OnDelete(DeleteBehavior.Cascade);

            // Agreeable Breed
            modelBuilder.Entity<AgreeableBreed>().HasKey(ab => new { ab.RoomId, ab.BreedId });

            modelBuilder.Entity<AgreeableBreed>()
                .HasOne(ab => ab.Room)
                .WithMany(rt => rt.AgreeableBreeds)
                .HasForeignKey(ab => ab.RoomId);


            // Room Accessory
            modelBuilder.Entity<RoomAccessory>().HasKey(ra => ra.RoomAccessoryId);
            modelBuilder.Entity<RoomAccessory>().Property(ra => ra.RoomAccessoryName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<RoomAccessory>().Property(ra => ra.RoomAccessoryDesc).HasMaxLength(500);

            modelBuilder.Entity<RoomAccessory>()
                .HasOne(ra => ra.RoomType)
                .WithMany(rt => rt.RoomAccessories)
                .HasForeignKey(ra => ra.RoomTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Room Image
            modelBuilder.Entity<RoomImage>().HasKey(ri => ri.RoomImagePath);

            // Room
            modelBuilder.Entity<Room>().HasKey(r => r.RoomId);
            modelBuilder.Entity<Room>().Property(r => r.RoomName).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Room>().Property(r => r.RoomDesc).HasMaxLength(500);
            modelBuilder.Entity<Room>().Property(r => r.PricePerHour).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Room>().Property(r => r.PricePerDay).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Room>()
                .HasOne(r => r.RoomType)
                .WithMany(rt => rt.Rooms)
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Room>()
                .HasMany(rt => rt.AgreeableBreeds)
                .WithOne(ab => ab.Room)
                .HasForeignKey(ab => ab.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // SEED DATA

            var roomTypeIds = Enumerable.Range(1, 5).Select(_ => Guid.NewGuid()).ToArray();

            modelBuilder.Entity<RoomType>().HasData(
                new RoomType { RoomTypeId = roomTypeIds[0], RoomTypeName = "Phòng Tiêu Chuẩn", RoomTypeDesc = "Phòng cơ bản cho thú cưng nhỏ", IsVisible = true },
                new RoomType { RoomTypeId = roomTypeIds[1], RoomTypeName = "Phòng VIP", RoomTypeDesc = "Phòng rộng rãi, tiện nghi cao cấp", IsVisible = true },
                new RoomType { RoomTypeId = roomTypeIds[2], RoomTypeName = "Phòng Gia Đình", RoomTypeDesc = "Dành cho nhiều thú cưng cùng ở", IsVisible = true }


            );

            // === RoomAccessories ===
            var accessoryIds = Enumerable.Range(1, 5).Select(_ => Guid.NewGuid()).ToArray();
            modelBuilder.Entity<RoomAccessory>().HasData(
                new RoomAccessory { RoomAccessoryId = accessoryIds[2], RoomAccessoryName = "Khay cát", RoomAccessoryDesc = "Khay vệ sinh cho mèo", RoomAccessoryImagePath = "khaycat.jpg", IsVisible = true, RoomTypeId = roomTypeIds[2], ServiceId = Guid.NewGuid() },
                new RoomAccessory { RoomAccessoryId = accessoryIds[3], RoomAccessoryName = "Cây leo", RoomAccessoryDesc = "Cho mèo chơi và tập thể dục", RoomAccessoryImagePath = "cayleo.jpg", IsVisible = true, RoomTypeId = roomTypeIds[1], ServiceId = Guid.NewGuid() },
                new RoomAccessory { RoomAccessoryId = accessoryIds[4], RoomAccessoryName = "Camera", RoomAccessoryDesc = "Quan sát thú cưng từ xa", RoomAccessoryImagePath = "camera.jpg", IsVisible = true, RoomTypeId = roomTypeIds[1], ServiceId = Guid.NewGuid() }
            );

            // === Rooms ===
            var roomIds = Enumerable.Range(1, 5).Select(_ => Guid.NewGuid()).ToArray();
            modelBuilder.Entity<Room>().HasData(
                new Room { RoomId = roomIds[0], RoomName = "Phòng 101", RoomDesc = "Phòng nhỏ, phù hợp chó con", PricePerHour = 50000, PricePerDay = 300000, IsVisible = true, RoomTypeId = roomTypeIds[0] },
                new Room { RoomId = roomIds[1], RoomName = "Phòng 202", RoomDesc = "Phòng VIP cho mèo quý tộc", PricePerHour = 80000, PricePerDay = 500000, IsVisible = true, RoomTypeId = roomTypeIds[1] },
                new Room { RoomId = roomIds[2], RoomName = "Phòng 303", RoomDesc = "Phòng gia đình, cho mèo", PricePerHour = 70000, PricePerDay = 450000, IsVisible = true, RoomTypeId = roomTypeIds[2] },
                new Room { RoomId = roomIds[3], RoomName = "Phòng 404", RoomDesc = "Phòng nhỏ cho mèo", PricePerHour = 90000, PricePerDay = 600000, IsVisible = true, RoomTypeId = roomTypeIds[0] }
            );

            // === RoomImages ===
            modelBuilder.Entity<RoomImage>().HasData(
                new RoomImage { RoomImagePath = "101.jpg", RoomId = roomIds[0] },
                new RoomImage { RoomImagePath = "202.jpg", RoomId = roomIds[1] },
                new RoomImage { RoomImagePath = "303.jpg", RoomId = roomIds[2] },
                new RoomImage { RoomImagePath = "404.jpg", RoomId = roomIds[3] },
                new RoomImage { RoomImagePath = "505.jpg", RoomId = roomIds[4] }
            );

            // === AgreeableBreeds ===
            modelBuilder.Entity<AgreeableBreed>().HasData(
                new AgreeableBreed { RoomId = roomIds[1], BreedId = Guid.Parse("aaae8a7b-abd1-4169-0383-08dd6a9d0b8b") },
                new AgreeableBreed { RoomId = roomIds[2], BreedId = Guid.Parse("aaae8a7b-abd1-4169-0383-08dd6a9d0b8b") },
                new AgreeableBreed { RoomId = roomIds[2], BreedId = Guid.Parse("f5614fe7-b506-4379-d3fa-08dd53047a20") },
                new AgreeableBreed { RoomId = roomIds[3], BreedId = Guid.Parse("aaae8a7b-abd1-4169-0383-08dd6a9d0b8b") },
                new AgreeableBreed { RoomId = roomIds[3], BreedId = Guid.Parse("f5614fe7-b506-4379-d3fa-08dd53047a20") }
            );

        }
    }
}
