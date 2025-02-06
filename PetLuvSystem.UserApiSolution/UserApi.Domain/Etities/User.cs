namespace UserApi.Domain.Etities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string? StaffType { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual ICollection<WorkSchedule>? WorkSchedules { get; set; }
        public virtual ICollection<StaffDegree>? StaffDegrees { get; set; }

    }
}
