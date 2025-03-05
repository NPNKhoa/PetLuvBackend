using Microsoft.EntityFrameworkCore;
using PaymentApi.Domain.Entities;

namespace PaymentApi.Infrastructure.Data
{
    public class PaymentDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Payment
            modelBuilder.Entity<Payment>().HasKey(p => p.PaymentId);
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.PaymentMethod)
                .WithMany(pm => pm.Payments)
                .HasForeignKey(p => p.PaymentMethodId);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.PaymentStatus)
                .WithMany(ps => ps.Payments)
                .HasForeignKey(p => p.PaymentStatusId);

            // PaymentMethod
            modelBuilder.Entity<PaymentMethod>().HasKey(pm => pm.PaymentMethodId);

            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod { PaymentMethodId = Guid.NewGuid(), PaymentMethodName = "Thanh toán qua VNPay", IsVisible = true },
                new PaymentMethod { PaymentMethodId = Guid.NewGuid(), PaymentMethodName = "Thanh toán tại cửa hàng", IsVisible = true },
                new PaymentMethod { PaymentMethodId = Guid.NewGuid(), PaymentMethodName = "Thanh toán khi nhận hàng", IsVisible = true }
            );

            // PaymentStatus
            modelBuilder.Entity<PaymentStatus>().HasKey(ps => ps.PaymentStatusId);

            modelBuilder.Entity<PaymentStatus>().HasData(
                new PaymentStatus { PaymentStatusId = Guid.NewGuid(), PaymentStatusName = "Chờ thanh toán", IsVisible = true },
                new PaymentStatus { PaymentStatusId = Guid.NewGuid(), PaymentStatusName = "Đã đặt cọc", IsVisible = true },
                new PaymentStatus { PaymentStatusId = Guid.NewGuid(), PaymentStatusName = "Đã thanh toán", IsVisible = true },
                new PaymentStatus { PaymentStatusId = Guid.NewGuid(), PaymentStatusName = "Thanh toán thất bại", IsVisible = true }
            );
        }
    }
}
