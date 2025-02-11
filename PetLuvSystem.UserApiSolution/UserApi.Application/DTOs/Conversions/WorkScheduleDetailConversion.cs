using UserApi.Application.DTOs.WorkScheduleDetailDTOs;
using UserApi.Domain.Etities;

namespace UserApi.Application.DTOs.Conversions
{
    public static class WorkScheduleDetailConversion
    {
        public static WorkScheduleDetail ToEntity(WorkScheduleDetailDTO dto) => new()
        {
            WorkingDate = dto.WorkingDate,
            WorkScheduleId = dto.WorkScheduleId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Note = dto.Note
        };

        public static (WorkScheduleDetailDTO?, IEnumerable<WorkScheduleDetailDTO>?) FromEntity(WorkScheduleDetail? entity, IEnumerable<WorkScheduleDetail>? entities)
        {
            if (entity is not null && entities is null)
            {
                return (new WorkScheduleDetailDTO(
                        entity.WorkingDate,
                        entity.WorkScheduleId,
                        entity.StartTime,
                        entity.EndTime,
                        entity.Note
                ), null);
            }

            if (entities is not null && entity is null)
            {
                return (null,
                    entities.Select(e => new WorkScheduleDetailDTO(
                        e.WorkingDate,
                        e.WorkScheduleId,
                        e.StartTime,
                        e.EndTime,
                        e.Note
                    )).ToList()
                );
            }

            return (null, null);
        }
    }
}
