namespace UserApi.Domain.Etities
{
    public class StaffDegree
    {
        public Guid DegreeId { get; set; }
        public string DegreeName { get; set; }
        public DateTime SignedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? DegreeDesc { get; set; }
        public string DegreeImage { get; set; }
        public bool IsVisible { get; set; }

        public Guid StaffId { get; set; }
        public virtual User Staff { get; set; }
    }
}
