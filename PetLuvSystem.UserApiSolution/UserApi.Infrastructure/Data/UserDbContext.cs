using Microsoft.EntityFrameworkCore;
using UserApi.Domain.Etities;

namespace UserApi.Infrastructure.Data
{
    public class UserDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<StaffDegree> StaffDegrees { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public DbSet<WorkScheduleDetail> WorkScheduleDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // StaffDegree
            modelBuilder.Entity<StaffDegree>()
                .HasKey(sd => sd.DegreeId);

            modelBuilder.Entity<StaffDegree>()
                .HasOne(sd => sd.Staff)
                .WithMany(u => u.StaffDegrees)
                .HasForeignKey(sd => sd.StaffId)
                .OnDelete(DeleteBehavior.Cascade);

            // WorkSchedule
            modelBuilder.Entity<WorkSchedule>()
                .HasKey(ws => ws.WorkScheduleId);

            modelBuilder.Entity<WorkSchedule>()
                .HasOne(ws => ws.Staff)
                .WithMany(u => u.WorkSchedules)
                .HasForeignKey(ws => ws.StaffId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkScheduleDetail>()
                .HasKey(wsd => new { wsd.WorkScheduleId, wsd.WorkingDate });

            // Work Schedule Detail
            modelBuilder.Entity<WorkScheduleDetail>()
                .HasOne<WorkSchedule>()
                .WithMany(ws => ws.WorkScheduleDetails)
                .HasForeignKey(wsd => wsd.WorkScheduleId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
