using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;

namespace ProductApi.Infrastructure.Data
{
    public class FoodDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodFlavor> FoodFlavors { get; set; }
        public DbSet<FoodSize> FoodSizes { get; set; }
        public DbSet<FoodVariant> FoodVariants { get; set; }
        public DbSet<FoodImage> FoodImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Food
            modelBuilder.Entity<Food>().HasKey(f => f.FoodId);
            modelBuilder.Entity<Food>().Property(f => f.CountInStock).HasPrecision(18, 2);

            modelBuilder.Entity<Food>().HasMany(f => f.FoodImages).WithOne(i => i.Food).HasForeignKey(i => i.FoodId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Food>().HasMany(f => f.FoodVariants).WithOne(v => v.Food).HasForeignKey(v => v.FoodId).OnDelete(DeleteBehavior.Cascade);

            // FoodFlavor
            modelBuilder.Entity<FoodFlavor>().HasKey(f => f.FlavorId);
            modelBuilder.Entity<FoodFlavor>().HasMany(f => f.FoodVariants).WithOne(v => v.Flavor).HasForeignKey(v => v.FlavorId);

            // FoodSize
            modelBuilder.Entity<FoodSize>().HasKey(s => s.SizeId);
            modelBuilder.Entity<FoodSize>().HasMany(s => s.FoodVariants).WithOne(v => v.Size).HasForeignKey(v => v.SizeId).OnDelete(DeleteBehavior.Cascade);

            // FoodVariant
            modelBuilder.Entity<FoodVariant>().HasKey(v => new { v.FoodId, v.FlavorId, v.SizeId });
            modelBuilder.Entity<FoodVariant>().Property(v => v.Price).HasPrecision(18, 2);

            // FoodImage
            modelBuilder.Entity<FoodImage>().HasKey(i => i.FoodImagePath);
            modelBuilder.Entity<FoodImage>().HasOne(i => i.Food).WithMany(f => f.FoodImages).HasForeignKey(i => i.FoodId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
