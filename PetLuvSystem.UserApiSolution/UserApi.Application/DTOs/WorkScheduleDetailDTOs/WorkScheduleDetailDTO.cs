namespace UserApi.Application.DTOs.WorkScheduleDetailDTOs
{
    public record WorkScheduleDetailDTO
    (
        DateTime WorkingDate,
        Guid WorkScheduleId,
        DateTime StartTime,
        DateTime EndTime,
        string? Note
    );
}
