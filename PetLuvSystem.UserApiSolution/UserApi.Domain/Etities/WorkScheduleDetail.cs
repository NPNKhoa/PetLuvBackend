namespace UserApi.Domain.Etities
{
    public class WorkScheduleDetail
    {
        public DateTime WorkingDate { get; set; }
        public Guid WorkScheduleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note { get; set; }

        public virtual WorkSchedule WorkSchedule { get; set; }
    }
}
