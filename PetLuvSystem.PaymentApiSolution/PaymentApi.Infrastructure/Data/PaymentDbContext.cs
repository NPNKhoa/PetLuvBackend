using Microsoft.EntityFrameworkCore;
using PaymentApi.Domain.Entities;

namespace PaymentApi.Infrastructure.Data
{
    public class PaymentDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<PaymentHistory> PaymentHistory { get; set; }

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

            // PaymentStatus
            modelBuilder.Entity<PaymentStatus>().HasKey(ps => ps.PaymentStatusId);

            // PaymentHistory
            modelBuilder.Entity<PaymentHistory>().HasKey(ph => ph.PaymentHistoryId);

            modelBuilder.Entity<PaymentHistory>()
                .HasOne(ph => ph.Payment)
                .WithMany(p => p.PaymentHistories)
                .HasForeignKey(ph => ph.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentHistory>()
                .HasOne(ph => ph.PaymentStatus)
                .WithMany(ps => ps.PaymentHistories)
                .HasForeignKey(ph => ph.PaymentStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
