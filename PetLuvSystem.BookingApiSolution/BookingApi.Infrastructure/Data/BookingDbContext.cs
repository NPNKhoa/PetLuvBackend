using BookingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Infrastructure.Data
{
    public class BookingDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingStatus> BookingStatuses { get; set; }
        public DbSet<BookingType> BookingTypes { get; set; }
        public DbSet<RoomBookingItem> RoomBookingItems { get; set; }
        public DbSet<ServiceBookingDetail> ServiceBookingDetails { get; set; }
        public DbSet<ServiceComboBookingDetail> ServiceComboBookingDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Booking
            modelBuilder.Entity<Booking>().HasKey(b => b.BookingId);

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .Property(b => b.DepositAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.BookingType)
                .WithMany(bt => bt.Bookings)
                .HasForeignKey(b => b.BookingTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.BookingStatus)
                .WithMany(bs => bs.Bookings)
                .HasForeignKey(b => b.BookingStatusId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.RoomBookingItem)
                .WithOne(rb => rb.Booking)
                .HasForeignKey<RoomBookingItem>(rb => rb.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasMany(b => b.ServiceBookingDetails)
                .WithOne(sb => sb.Booking)
                .HasForeignKey(sb => sb.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasMany(b => b.ServiceComboBookingDetails)
                .WithOne(sb => sb.Booking)
                .HasForeignKey(sb => sb.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // BookingStatus
            modelBuilder.Entity<BookingStatus>().HasKey(bs => bs.BookingStatusId);

            // BooingType
            modelBuilder.Entity<BookingType>().HasKey(bs => bs.BookingTypeId);

            // RoomBookingItem
            modelBuilder.Entity<RoomBookingItem>()
               .HasKey(r => new { r.BookingId, r.RoomId });

            modelBuilder.Entity<RoomBookingItem>()
            .Property(b => b.ItemPrice)
            .HasPrecision(18, 2);

            // ServiceBookingDetail
            modelBuilder.Entity<ServiceBookingDetail>()
               .HasKey(r => new { r.BookingId, r.ServiceId, r.BreedId, r.PetWeightRange });

            modelBuilder.Entity<ServiceBookingDetail>()
                .Property(b => b.BookingItemPrice)
                .HasPrecision(18, 2);

            // ServiceComboBookingDetail
            modelBuilder.Entity<ServiceComboBookingDetail>()
               .HasKey(r => new { r.BookingId, r.ServiceComboId });

            modelBuilder.Entity<ServiceComboBookingDetail>()
                .Property(b => b.BookingItemPrice)
                .HasPrecision(18, 2);

            // SEED
            modelBuilder.Entity<BookingStatus>().HasData(
                new BookingStatus
                {
                    BookingStatusId = Guid.NewGuid(),
                    BookingStatusName = "Đang xử lý",
                    IsVisible = true
                }
            );
        }

    }
}
