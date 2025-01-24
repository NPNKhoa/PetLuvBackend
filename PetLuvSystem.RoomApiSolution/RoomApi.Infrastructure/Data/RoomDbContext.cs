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
            modelBuilder.Entity<RoomType>()
                .HasMany(rt => rt.AgreeableBreeds)
                .WithOne(ab => ab.RoomType)
                .HasForeignKey(ab => ab.RoomTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Agreeable Breed
            modelBuilder.Entity<AgreeableBreed>().HasKey(ab => new { ab.RoomTypeId, ab.BreedId });

            modelBuilder.Entity<AgreeableBreed>()
                .HasOne(ab => ab.RoomType)
                .WithMany(rt => rt.AgreeableBreeds)
                .HasForeignKey(ab => ab.RoomTypeId);


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
        }
    }
}
