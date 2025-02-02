using Microsoft.EntityFrameworkCore;
using PetApi.Domain.Entities;

namespace PetApi.Infrastructure.Data
{
    public class PetDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetBreed> PetBreeds { get; set; }
        public DbSet<PetHealthBook> PetHealthBooks { get; set; }
        public DbSet<PetHealthBookDetail> PetHealthBookDetails { get; set; }
        public DbSet<PetImage> PetImages { get; set; }
        public DbSet<PetType> PetTypes { get; set; }
        public DbSet<SellingPet> SellingPets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Pet
            modelBuilder.Entity<Pet>().HasKey(p => p.PetId);

            modelBuilder.Entity<Pet>()
                .HasOne(p => p.ParentPet)
                .WithMany(p => p.ChildrenPets)
                .HasForeignKey(p => p.ParentPetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pet>()
                .HasOne(p => p.PetBreed)
                .WithMany(b => b.Pets)
                .HasForeignKey(p => p.BreedId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pet>()
                .HasMany(p => p.PetImagePaths)
                .WithOne()
                .HasForeignKey("PetId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pet>()
                .HasMany(p => p.PetHealthBooks)
                .WithOne()
                .HasForeignKey("PetId")
                .OnDelete(DeleteBehavior.Cascade);

            // PetBreed
            modelBuilder.Entity<PetBreed>().HasKey(p => p.BreedId);
            modelBuilder.Entity<PetBreed>()
                .HasOne(b => b.PetType)
                .WithMany(t => t.PetBreeds)
                .HasForeignKey(b => b.PetTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            // PetHealthBook
            modelBuilder.Entity<PetHealthBook>().HasKey(h => h.HealthBookId);
            modelBuilder.Entity<PetHealthBook>()
                .HasMany(h => h.PetHealthBookDetails)
                .WithOne(d => d.PetHealthBook)
                .HasForeignKey(d => d.HealthBookId)
                .OnDelete(DeleteBehavior.Cascade);

            // PetHealthBookDetail
            modelBuilder.Entity<PetHealthBookDetail>()
                .HasKey(d => new { d.HealthBookDetailId, d.HealthBookId });

            // PetImage
            modelBuilder.Entity<PetImage>()
                .HasKey(p => p.PetImagePath);

            // PetType
            modelBuilder.Entity<PetType>().HasKey(t => t.PetTypeId);
            modelBuilder.Entity<PetType>()
                .HasMany(t => t.PetBreeds)
                .WithOne(b => b.PetType)
                .HasForeignKey(b => b.PetTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            // SellingPet
            modelBuilder.Entity<SellingPet>()
                .HasBaseType<Pet>();

            modelBuilder.Entity<Pet>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Pet>("Pet")
                .HasValue<SellingPet>("SellingPet");

            modelBuilder.Entity<SellingPet>().Property(p => p.SellingPrice).HasPrecision(18, 2);
        }

    }
}
