using UserApi.Application.DTOs.UserDTOs;
using UserApi.Application.DTOs.WorkScheduleDetailDTOs;
using UserApi.Application.DTOs.WorkScheduleDTOs;
using UserApi.Domain.Etities;

namespace UserApi.Application.DTOs.Conversions
{
    public static class WorkScheduleConversion
    {
        public static WorkSchedule ToEntity(CreateUpdateWorkScheduleDTO dto) => new()
        {
            WorkScheduleId = Guid.Empty,
            StaffId = dto.StaffId
        };

        public static (WorkScheduleDTO?, IEnumerable<WorkScheduleDTO>?) FromEntity(WorkSchedule? entity, IEnumerable<WorkSchedule>? entities)
        {
            if (entity is not null && entities is null)
            {
                return (new WorkScheduleDTO(
                    entity.WorkScheduleId,
                    new BriefUserDTO(
                        entity.Staff.UserId,
                        entity.Staff.FullName,
                        entity.Staff.Email,
                        entity.Staff.PhoneNumber,
                        entity.Staff.Avatar,
                        entity.Staff!.StaffType!,
                        entity.Staff.IsActive
                    ),
                    entity.WorkScheduleDetails.Select(e => new WorkScheduleDetailDTO(
                        e.WorkingDate,
                        e.WorkScheduleId,
                        e.StartTime,
                        e.EndTime,
                        e.Note
                    )).ToList()
                ), null);
            }

            if (entities is not null && entity is null)
            {
                return (null,
                    entities.Select(e => new WorkScheduleDTO(
                        e.WorkScheduleId,
                        new BriefUserDTO(
                            e.Staff.UserId,
                            e.Staff.FullName,
                            e.Staff.Email,
                            e.Staff.PhoneNumber,
                            e.Staff.Avatar,
                            e.Staff!.StaffType!,
                            e.Staff.IsActive
                        ),
                        e.WorkScheduleDetails.Select(x => new WorkScheduleDetailDTO(
                            x.WorkingDate,
                            x.WorkScheduleId,
                            x.StartTime,
                            x.EndTime,
                            x.Note
                        )).ToList()
                    )).ToList()
                );
            }

            return (null, null);
        }
    }
}
