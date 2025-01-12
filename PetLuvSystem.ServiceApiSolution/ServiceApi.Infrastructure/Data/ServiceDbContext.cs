using Microsoft.EntityFrameworkCore;
using ServiceApi.Domain.Entities;

namespace ServiceApi.Infrastructure.Data
{
    public class ServiceDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceCombo> ServiceCombos { get; set; }
        public DbSet<ServiceComboMapping> ServiceComboMappings { get; set; }
        public DbSet<ServiceComboVariant> ServiceComboPrices { get; set; }
        public DbSet<ServiceImage> ServiceImages { get; set; }
        public DbSet<ServiceVariant> ServicePrices { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<WalkDogServiceVariant> WalkDogServicePrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Service
            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(s => s.ServiceName)
                    .HasMaxLength(100)
                    .IsRequired();
                entity.Property(s => s.ServiceDesc)
                    .HasMaxLength(500);
                entity.HasOne(s => s.ServiceType)
                    .WithMany(st => st.Services)
                    .HasForeignKey(s => s.ServiceTypeId)
                    .OnDelete(DeleteBehavior.Cascade);


                entity.HasMany(s => s.ServiceComboMappings)
                    .WithOne(scm => scm.Service)
                    .HasForeignKey(scm => scm.ServiceId);
            });

            // ServiceComboMapping
            modelBuilder.Entity<ServiceComboMapping>()
                .HasKey(scm => new { scm.ServiceId, scm.ServiceComboId });

            modelBuilder.Entity<ServiceComboMapping>()
                .HasOne(scm => scm.Service)
                .WithMany(s => s.ServiceComboMappings)
                .HasForeignKey(scm => scm.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceComboMapping>()
                .HasOne(scm => scm.ServiceCombo)
                .WithMany(sc => sc.ServiceComboMappings)
                .HasForeignKey(scm => scm.ServiceComboId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceComboPrice
            modelBuilder.Entity<ServiceComboVariant>()
                .HasKey(scp => new { scp.ServiceComboId, scp.BreedId, scp.WeightRange });

            modelBuilder.Entity<ServiceComboVariant>()
                .HasOne(scp => scp.ServiceCombo)
                .WithMany(sc => sc.ServiceComboPrices)
                .HasForeignKey(scp => scp.ServiceComboId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServicePrice
            modelBuilder.Entity<ServiceVariant>()
                .HasKey(sp => new { sp.ServiceId, sp.BreedId, sp.PetWeightRange });

            modelBuilder.Entity<ServiceVariant>()
                .HasOne(sp => sp.Service)
                .WithMany(s => s.ServiceVariants)
                .HasForeignKey(sp => sp.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // WalkDogServicePrice
            modelBuilder.Entity<WalkDogServiceVariant>()
                .HasKey(wdsp => wdsp.ServiceId);

            modelBuilder.Entity<WalkDogServiceVariant>()
                .HasOne(wdsp => wdsp.Service)
                .WithMany(s => s.WalkDogServiceVariants)
                .HasForeignKey(wdsp => wdsp.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceImage
            modelBuilder.Entity<ServiceImage>()
                .HasKey(si => si.ServiceImagePath);

            modelBuilder.Entity<ServiceImage>()
                .HasOne(si => si.Service)
                .WithMany(s => s.ServiceImages)
                .HasForeignKey(si => si.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Add data for ServiceType
            modelBuilder.Entity<ServiceType>().HasData(
                new ServiceType { ServiceTypeId = Guid.NewGuid(), ServiceTypeName = "Dịch vụ spa", ServiceTypeDesc = "Cung cấp các dịch vụ spa cho thú cưng trực tiếp tại cửa hàng" },
                new ServiceType
                {
                    ServiceTypeId = Guid.NewGuid(),
                    ServiceTypeName = "Dịch vụ spa tại nhà",
                    ServiceTypeDesc = "Chúng tôi sẵn sàng trực tiếp đến nhà chăm sóc cho \"boss\" của bạn nếu bạn đang quá bận"
                },
                new ServiceType
                {
                    ServiceTypeId = Guid.NewGuid(),
                    ServiceTypeName = "Dịch vụ dắt chó đi dạo",
                    ServiceTypeDesc = "Nếu bạn đang quá bận để giúp cún cưng của mình giải tỏa năng lượng, hãy để chúng tôi giúp bạn làm điều đó"
                }
            );
        }
    }
}
