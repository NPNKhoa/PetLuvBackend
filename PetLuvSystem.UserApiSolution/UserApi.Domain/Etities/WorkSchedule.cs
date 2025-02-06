namespace UserApi.Domain.Etities
{
    public class WorkSchedule
    {
        public Guid WorkScheduleId { get; set; }

        public Guid StaffId { get; set; }
        public virtual User Staff { get; set; }

        public virtual ICollection<WorkScheduleDetail> WorkScheduleDetails { get; set; }
    }
}
